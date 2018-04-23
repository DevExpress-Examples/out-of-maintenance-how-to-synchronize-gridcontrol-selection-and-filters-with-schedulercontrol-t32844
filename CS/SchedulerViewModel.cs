using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using DevExpress.XtraScheduler;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Grid;
using System.Collections.Generic;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;


namespace SchedulerBindToObservableCollectionWpf {
    public class SchedulerViewModel: ViewModelBase {
        public ICommand SchedulerLoadedCommand { get; private set; }
        public ICommand FilterChangedCommand { get; private set; }
        public ICommand ShowEditFormCommand { get; private set; }
        public SchedulerControl Scheduler { get; set; }
        public virtual ObservableCollection<ModelAppointment> Appointments { get; private set; }
        public virtual ObservableCollection<ModelResource> Resources { get; private set; }
        public virtual ObservableCollection<Appointment> SelectedAppointments {get; set;}
        public virtual ObservableCollection<ModelAppointment> SelectedModelAppointments { get; set; }
        private ModelAppointment currentModelAppointment;
        public ModelAppointment CurrentModelAppointment
        {
            get { return currentModelAppointment; } 
            set
            {
                if(value != currentModelAppointment)
                {
                    currentModelAppointment = value;
                    ScrollToAppointment();
                }
            }
        }
        public SchedulerViewModel() {
            Appointments = new ObservableCollection<ModelAppointment>();
            Resources = new ObservableCollection<ModelResource>();
            
            SelectedAppointments = new ObservableCollection<Appointment>();
            SelectedModelAppointments = new ObservableCollection<ModelAppointment>();

            SelectedAppointments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(SelectedAppointments_CollectionChanged);
            SelectedModelAppointments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(SelectedModelAppointment_CollectionChanged);
            
            SchedulerLoadedCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(SchedulerLoadedCommandExecute);
            FilterChangedCommand = new DevExpress.Mvvm.DelegateCommand<GridEventArgs>(FilterChangedCommandExecute);
            ShowEditFormCommand = new DevExpress.Mvvm.DelegateCommand<RowDoubleClickEventArgs>(ShowEditFormCommandExecute);

            DataProvider.AddTestData(Resources, Appointments);
        }
        protected void ScrollToAppointment()
        {
            if (Scheduler != null)
            {
                Scheduler.ActiveView.GotoTimeInterval(new TimeInterval(CurrentModelAppointment.StartTime, CurrentModelAppointment.EndTime));
                if (Scheduler.ActiveViewType == SchedulerViewType.Day)
                    (Scheduler.ActiveView as DayView).TopRowTime = CurrentModelAppointment.StartTime.TimeOfDay;
            }
        }
        private void ShowEditFormCommandExecute(RowDoubleClickEventArgs args)
        {
            Appointment currentAppointment = Scheduler.Storage.AppointmentStorage.GetAppointmentById(CurrentModelAppointment.Id);
            if (currentAppointment != null)
                Scheduler.ShowEditAppointmentForm(currentAppointment);
        }

        private void SchedulerLoadedCommandExecute(RoutedEventArgs args)
        {
            Scheduler = args.Source as SchedulerControl;
            Scheduler.Start = DateTime.Today;
            Scheduler.Storage.AppointmentInserting += Storage_AppointmentInserting;
        }
        private void FilterChangedCommandExecute(GridEventArgs args)
        {
            GridControl grid = args.Source as GridControl;
            if (grid.IsFilterEnabled)
                Scheduler.Storage.AppointmentStorage.Filter = grid.FilterString;
            else
                Scheduler.Storage.AppointmentStorage.Filter = "";
        }
        void Storage_AppointmentInserting(object sender, PersistentObjectCancelEventArgs e)
        {
            Scheduler.Storage.SetAppointmentId((Appointment)e.Object, Guid.NewGuid());
        }
        void SelectedAppointments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SelectedModelAppointments.CollectionChanged -= SelectedModelAppointment_CollectionChanged;
            SelectGridRows();
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                CurrentModelAppointment = (e.NewItems[0] as Appointment).GetSourceObject(Scheduler.Storage.GetCoreStorage()) as ModelAppointment;
                RaisePropertyChanged("CurrentModelAppointment");
            }
            SelectedModelAppointments.CollectionChanged += SelectedModelAppointment_CollectionChanged;
        }
        private void SelectedModelAppointment_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SelectedAppointments.CollectionChanged -= SelectedAppointments_CollectionChanged;
            SelectAppointments();
            SelectedAppointments.CollectionChanged += SelectedAppointments_CollectionChanged;
        }
        void SelectGridRows()
        {
            SelectedModelAppointments.Clear();
            foreach (Appointment apt in SelectedAppointments)
            {
                if (apt.IsBase || apt.IsException)
                    SelectedModelAppointments.Add(apt.GetSourceObject(Scheduler.Storage.GetCoreStorage()) as ModelAppointment);
                if(apt.IsOccurrence && !apt.IsException)
                    SelectedModelAppointments.Add(apt.RecurrencePattern.GetSourceObject(Scheduler.Storage.GetCoreStorage()) as ModelAppointment);
            }
        }
        void SelectAppointments()
        {
            SelectedAppointments.Clear();
            foreach (ModelAppointment modelApt in SelectedModelAppointments)
            {
                Appointment apt = Scheduler.Storage.AppointmentStorage.GetAppointmentById(modelApt.Id);
                if(apt != null)
                    SelectedAppointments.Add(apt);
            }
        }
    }
}