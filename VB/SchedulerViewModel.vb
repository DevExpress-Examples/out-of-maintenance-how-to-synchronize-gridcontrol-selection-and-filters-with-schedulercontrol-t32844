Imports System
Imports System.Windows
Imports System.Windows.Input
Imports System.Collections.ObjectModel
Imports DevExpress.XtraScheduler
Imports DevExpress.Xpf.Scheduler
Imports DevExpress.Xpf.Grid
Imports System.Collections.Generic
Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.DataAnnotations


Namespace SchedulerBindToObservableCollectionWpf
	Public Class SchedulerViewModel
		Inherits ViewModelBase

		Private privateSchedulerLoadedCommand As ICommand
		Public Property SchedulerLoadedCommand() As ICommand
			Get
				Return privateSchedulerLoadedCommand
			End Get
			Private Set(ByVal value As ICommand)
				privateSchedulerLoadedCommand = value
			End Set
		End Property
		Private privateFilterChangedCommand As ICommand
		Public Property FilterChangedCommand() As ICommand
			Get
				Return privateFilterChangedCommand
			End Get
			Private Set(ByVal value As ICommand)
				privateFilterChangedCommand = value
			End Set
		End Property
		Private privateShowEditFormCommand As ICommand
		Public Property ShowEditFormCommand() As ICommand
			Get
				Return privateShowEditFormCommand
			End Get
			Private Set(ByVal value As ICommand)
				privateShowEditFormCommand = value
			End Set
		End Property
		Public Property Scheduler() As SchedulerControl
		Private privateAppointments As ObservableCollection(Of ModelAppointment)
		Public Overridable Property Appointments() As ObservableCollection(Of ModelAppointment)
			Get
				Return privateAppointments
			End Get
			Private Set(ByVal value As ObservableCollection(Of ModelAppointment))
				privateAppointments = value
			End Set
		End Property
		Private privateResources As ObservableCollection(Of ModelResource)
		Public Overridable Property Resources() As ObservableCollection(Of ModelResource)
			Get
				Return privateResources
			End Get
			Private Set(ByVal value As ObservableCollection(Of ModelResource))
				privateResources = value
			End Set
		End Property
		Public Overridable Property SelectedAppointments() As ObservableCollection(Of Appointment)
		Public Overridable Property SelectedModelAppointments() As ObservableCollection(Of ModelAppointment)
'INSTANT VB NOTE: The field currentModelAppointment was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private currentModelAppointment_Conflict As ModelAppointment
		Public Property CurrentModelAppointment() As ModelAppointment
			Get
				Return currentModelAppointment_Conflict
			End Get
			Set(ByVal value As ModelAppointment)
				If value IsNot currentModelAppointment_Conflict Then
					currentModelAppointment_Conflict = value
					ScrollToAppointment()
				End If
			End Set
		End Property
		Public Sub New()
			Appointments = New ObservableCollection(Of ModelAppointment)()
			Resources = New ObservableCollection(Of ModelResource)()

			SelectedAppointments = New ObservableCollection(Of Appointment)()
			SelectedModelAppointments = New ObservableCollection(Of ModelAppointment)()

			AddHandler SelectedAppointments.CollectionChanged, AddressOf SelectedAppointments_CollectionChanged
			AddHandler SelectedModelAppointments.CollectionChanged, AddressOf SelectedModelAppointment_CollectionChanged

			SchedulerLoadedCommand = New DevExpress.Mvvm.DelegateCommand(Of RoutedEventArgs)(AddressOf SchedulerLoadedCommandExecute)
			FilterChangedCommand = New DevExpress.Mvvm.DelegateCommand(Of GridEventArgs)(AddressOf FilterChangedCommandExecute)
			ShowEditFormCommand = New DevExpress.Mvvm.DelegateCommand(Of RowDoubleClickEventArgs)(AddressOf ShowEditFormCommandExecute)

			DataProvider.AddTestData(Resources, Appointments)
		End Sub
		Protected Sub ScrollToAppointment()
			If Scheduler IsNot Nothing Then
				Scheduler.ActiveView.GotoTimeInterval(New TimeInterval(CurrentModelAppointment.StartTime, CurrentModelAppointment.EndTime))
				If Scheduler.ActiveViewType = SchedulerViewType.Day Then
					TryCast(Scheduler.ActiveView, DayView).TopRowTime = CurrentModelAppointment.StartTime.TimeOfDay
				End If
			End If
		End Sub
		Private Sub ShowEditFormCommandExecute(ByVal args As RowDoubleClickEventArgs)
			Dim currentAppointment As Appointment = Scheduler.Storage.AppointmentStorage.GetAppointmentById(CurrentModelAppointment.Id)
			If currentAppointment IsNot Nothing Then
				Scheduler.ShowEditAppointmentForm(currentAppointment)
			End If
		End Sub

		Private Sub SchedulerLoadedCommandExecute(ByVal args As RoutedEventArgs)
			Scheduler = TryCast(args.Source, SchedulerControl)
			Scheduler.Start = DateTime.Today
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
		Private Sub SelectedAppointments_CollectionChanged(ByVal sender As Object, ByVal e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
			RemoveHandler SelectedModelAppointments.CollectionChanged, AddressOf SelectedModelAppointment_CollectionChanged
			SelectGridRows()
			If e.Action = System.Collections.Specialized.NotifyCollectionChangedAction.Add Then
				CurrentModelAppointment = TryCast((TryCast(e.NewItems(0), Appointment)).GetSourceObject(Scheduler.Storage.GetCoreStorage()), ModelAppointment)
				RaisePropertyChanged("CurrentModelAppointment")
			End If
			AddHandler SelectedModelAppointments.CollectionChanged, AddressOf SelectedModelAppointment_CollectionChanged
		End Sub
		Private Sub SelectedModelAppointment_CollectionChanged(ByVal sender As Object, ByVal e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
			RemoveHandler SelectedAppointments.CollectionChanged, AddressOf SelectedAppointments_CollectionChanged
			SelectAppointments()
			AddHandler SelectedAppointments.CollectionChanged, AddressOf SelectedAppointments_CollectionChanged
		End Sub
		Private Sub SelectGridRows()
			SelectedModelAppointments.Clear()
			For Each apt As Appointment In SelectedAppointments
				If apt.IsBase OrElse apt.IsException Then
					SelectedModelAppointments.Add(TryCast(apt.GetSourceObject(Scheduler.Storage.GetCoreStorage()), ModelAppointment))
				End If
				If apt.IsOccurrence AndAlso Not apt.IsException Then
					SelectedModelAppointments.Add(TryCast(apt.RecurrencePattern.GetSourceObject(Scheduler.Storage.GetCoreStorage()), ModelAppointment))
				End If
			Next apt
		End Sub
		Private Sub SelectAppointments()
			SelectedAppointments.Clear()
			For Each modelApt As ModelAppointment In SelectedModelAppointments
				Dim apt As Appointment = Scheduler.Storage.AppointmentStorage.GetAppointmentById(modelApt.Id)
				If apt IsNot Nothing Then
					SelectedAppointments.Add(apt)
				End If
			Next modelApt
		End Sub
	End Class
End Namespace