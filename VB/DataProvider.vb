Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace SchedulerBindToObservableCollectionWpf
	Public Module DataProvider
'INSTANT VB NOTE: The parameter resources was renamed since it may cause conflicts with calls to static members of the user-defined type with this name:
		Public Sub AddTestData(ByVal resources_Conflict As ObservableCollection(Of ModelResource), ByVal appointments As ObservableCollection(Of ModelAppointment))
			Dim res1 As New ModelResource() With {
				.Id = 0,
				.Name = "Computer1",
				.Color = ToRgb(System.Drawing.Color.Yellow)
			}

			Dim res2 As New ModelResource() With {
				.Id = 1,
				.Name = "Computer2",
				.Color = ToRgb(System.Drawing.Color.Green)
			}

			Dim res3 As New ModelResource() With {
				.Id = 2,
				.Name = "Computer3",
				.Color = ToRgb(System.Drawing.Color.Blue)
			}

			resources_Conflict.Add(res1)
			resources_Conflict.Add(res2)
			resources_Conflict.Add(res3)

			Dim baseTime As DateTime = DateTime.Now

			Dim apt1 As New ModelAppointment() With {
				.Id = Guid.NewGuid(),
				.StartTime = baseTime.AddHours(-2),
				.EndTime = baseTime.AddHours(-1),
				.Subject = "Test 1",
				.Location = "Office",
				.Description = "Test procedure 1",
				.Price = 10D,
				.ResourceId = 0
			}

			Dim apt2 As New ModelAppointment() With {
				.Id = Guid.NewGuid(),
				.StartTime = baseTime,
				.EndTime = baseTime.AddHours(2),
				.Subject = "Test 2",
				.Location = "Office",
				.Description = "Test procedure 2",
				.ResourceId = 1
			}

			Dim apt3 As New ModelAppointment() With {
				.Id = Guid.NewGuid(),
				.StartTime = baseTime.AddHours(-1),
				.EndTime = baseTime.AddHours(1),
				.Subject = "Test 3",
				.Location = "Office",
				.Description = "Test procedure 3",
				.ResourceId = 2
			}

			appointments.Add(apt1)
			appointments.Add(apt2)
			appointments.Add(apt3)
		End Sub
		Private Function ToRgb(ByVal color As System.Drawing.Color) As Integer
			Return color.B << 16 Or color.G << 8 Or color.R
		End Function
	End Module
End Namespace
