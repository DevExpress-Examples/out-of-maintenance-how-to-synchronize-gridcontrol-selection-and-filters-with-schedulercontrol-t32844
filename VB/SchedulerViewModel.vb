Imports System
Imports System.Windows
Imports System.Windows.Input
Imports System.Collections.ObjectModel
Imports DevExpress.XtraScheduler
Imports DevExpress.Xpf.Scheduler
Imports DevExpress.Xpf.Grid
Imports DevExpress.Mvvm

Namespace SchedulerBindToObservableCollectionWpf

    Public Class SchedulerViewModel
        Inherits ViewModelBase

        Private _SchedulerLoadedCommand As ICommand, _FilterChangedCommand As ICommand, _ShowEditFormCommand As ICommand

        Public Property SchedulerLoadedCommand As ICommand
            Get
                Return _SchedulerLoadedCommand
            End Get

            Private Set(ByVal value As ICommand)
                _SchedulerLoadedCommand = value
            End Set
        End Property

        Public Property FilterChangedCommand As ICommand
            Get
                Return _FilterChangedCommand
            End Get

            Private Set(ByVal value As ICommand)
                _FilterChangedCommand = value
            End Set
        End Property

        Public Property ShowEditFormCommand As ICommand
            Get
                Return _ShowEditFormCommand
            End Get

            Private Set(ByVal value As ICommand)
                _ShowEditFormCommand = value
            End Set
        End Property

        Public Property Scheduler As SchedulerControl

        Public Overridable Property Appointments As ObservableCollection(Of ModelAppointment)

        Public Overridable Property Resources As ObservableCollection(Of ModelResource)

        Public Overridable Property SelectedAppointments As ObservableCollection(Of Appointment)

        Public Overridable Property SelectedModelAppointments As ObservableCollection(Of ModelAppointment)

        Private currentModelAppointmentField As ModelAppointment

        Public Property CurrentModelAppointment As ModelAppointment
            Get
                Return currentModelAppointmentField
            End Get

            Set(ByVal value As ModelAppointment)
                If value IsNot currentModelAppointmentField Then
                    currentModelAppointmentField = value
                    ScrollToAppointment()
                End If
            End Set
        End Property

        Public Sub New()
            Appointments = New ObservableCollection(Of ModelAppointment)()
            Resources = New ObservableCollection(Of ModelResource)()
            SelectedAppointments = New ObservableCollection(Of Appointment)()
            SelectedModelAppointments = New ObservableCollection(Of ModelAppointment)()
            AddHandler SelectedAppointments.CollectionChanged, New Collections.Specialized.NotifyCollectionChangedEventHandler(AddressOf SelectedAppointments_CollectionChanged)
            AddHandler SelectedModelAppointments.CollectionChanged, New Collections.Specialized.NotifyCollectionChangedEventHandler(AddressOf SelectedModelAppointment_CollectionChanged)
            SchedulerLoadedCommand = New DelegateCommand(Of RoutedEventArgs)(AddressOf SchedulerLoadedCommandExecute)
            FilterChangedCommand = New DelegateCommand(Of GridEventArgs)(AddressOf FilterChangedCommandExecute)
            ShowEditFormCommand = New DelegateCommand(Of RowDoubleClickEventArgs)(AddressOf ShowEditFormCommandExecute)
            AddTestData(Resources, Appointments)
        End Sub

        Protected Sub ScrollToAppointment()
            If Scheduler IsNot Nothing Then
                Scheduler.ActiveView.GotoTimeInterval(New TimeInterval(CurrentModelAppointment.StartTime, CurrentModelAppointment.EndTime))
                If Scheduler.ActiveViewType = SchedulerViewType.Day Then TryCast(Scheduler.ActiveView, DayView).TopRowTime = CurrentModelAppointment.StartTime.TimeOfDay
            End If
        End Sub

        Private Sub ShowEditFormCommandExecute(ByVal args As RowDoubleClickEventArgs)
            Dim currentAppointment As Appointment = Scheduler.Storage.AppointmentStorage.GetAppointmentById(CurrentModelAppointment.Id)
            If currentAppointment IsNot Nothing Then Scheduler.ShowEditAppointmentForm(currentAppointment)
        End Sub

        Private Sub SchedulerLoadedCommandExecute(ByVal args As RoutedEventArgs)
            Scheduler = TryCast(args.Source, SchedulerControl)
            Scheduler.Start = Date.Today
            AddHandler Scheduler.Storage.AppointmentInserting, AddressOf Storage_AppointmentInserting
        End Sub

        Private Sub FilterChangedCommandExecute(ByVal args As GridEventArgs)
            Dim grid As GridControl = TryCast(args.Source, GridControl)
            If grid.IsFilterEnabled Then
                Scheduler.Storage.AppointmentStorage.Filter = grid.FilterString
            Else
                Scheduler.Storage.AppointmentStorage.Filter = ""
            End If
        End Sub

        Private Sub Storage_AppointmentInserting(ByVal sender As Object, ByVal e As PersistentObjectCancelEventArgs)
            Scheduler.Storage.SetAppointmentId(CType(e.Object, Appointment), Guid.NewGuid())
        End Sub

        Private Sub SelectedAppointments_CollectionChanged(ByVal sender As Object, ByVal e As Collections.Specialized.NotifyCollectionChangedEventArgs)
            RemoveHandler SelectedModelAppointments.CollectionChanged, AddressOf SelectedModelAppointment_CollectionChanged
            SelectGridRows()
            If e.Action = Collections.Specialized.NotifyCollectionChangedAction.Add Then
                CurrentModelAppointment = TryCast(TryCast(e.NewItems(0), Appointment).GetSourceObject(Scheduler.Storage.GetCoreStorage()), ModelAppointment)
                RaisePropertyChanged("CurrentModelAppointment")
            End If

            AddHandler SelectedModelAppointments.CollectionChanged, AddressOf SelectedModelAppointment_CollectionChanged
        End Sub

        Private Sub SelectedModelAppointment_CollectionChanged(ByVal sender As Object, ByVal e As Collections.Specialized.NotifyCollectionChangedEventArgs)
            RemoveHandler SelectedAppointments.CollectionChanged, AddressOf SelectedAppointments_CollectionChanged
            SelectAppointments()
            AddHandler SelectedAppointments.CollectionChanged, AddressOf SelectedAppointments_CollectionChanged
        End Sub

        Private Sub SelectGridRows()
            SelectedModelAppointments.Clear()
            For Each apt As Appointment In SelectedAppointments
                If apt.IsBase OrElse apt.IsException Then SelectedModelAppointments.Add(TryCast(apt.GetSourceObject(Scheduler.Storage.GetCoreStorage()), ModelAppointment))
                If apt.IsOccurrence AndAlso Not apt.IsException Then SelectedModelAppointments.Add(TryCast(apt.RecurrencePattern.GetSourceObject(Scheduler.Storage.GetCoreStorage()), ModelAppointment))
            Next
        End Sub

        Private Sub SelectAppointments()
            SelectedAppointments.Clear()
            For Each modelApt As ModelAppointment In SelectedModelAppointments
                Dim apt As Appointment = Scheduler.Storage.AppointmentStorage.GetAppointmentById(modelApt.Id)
                If apt IsNot Nothing Then SelectedAppointments.Add(apt)
            Next
        End Sub
    End Class
End Namespace
