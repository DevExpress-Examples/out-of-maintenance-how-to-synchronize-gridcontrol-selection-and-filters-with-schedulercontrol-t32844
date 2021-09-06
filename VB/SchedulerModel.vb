Imports System

Namespace SchedulerBindToObservableCollectionWpf
#Region "#models    "
	Public Class ModelAppointment
		Public Property Id() As Object
		Public Property StartTime() As DateTime
		Public Property EndTime() As DateTime
		Public Property Subject() As String
		Public Property Status() As Integer
		Public Property Description() As String
		Public Property Label() As Integer
		Public Property Location() As String
		Public Property AllDay() As Boolean
		Public Property EventType() As Integer
		Public Property RecurrenceInfo() As String
		Public Property ReminderInfo() As String
		Public Property ResourceId() As Object
		Public Property Price() As Decimal
		Public Sub New()
		End Sub
	End Class

	Public Class ModelResource
		Public Property Id() As Integer
		Public Property Name() As String
		Public Property Color() As Integer

		Public Sub New()
		End Sub
	End Class
#End Region ' #models
End Namespace
