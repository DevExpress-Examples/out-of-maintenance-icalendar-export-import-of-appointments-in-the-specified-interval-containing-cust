Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Collections
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.iCalendar
Imports DevExpress.XtraScheduler.Drawing


Namespace iCalendarExportImport
	Partial Public Class Form1
		Inherits Form
		Private Const CustomFieldName As String = "MyCustomField"
		Private Const CustomPropertySignature As String = "JohnDoeInc"
		Public iCalendarFileName As String = ""



		Public Sub New()
			InitializeComponent()

			schedulerControl1.Start = DateTime.Now
			schedulerControl1.DayView.DayCount = 5

			Dim mapping As New AppointmentCustomFieldMapping(CustomFieldName, CustomFieldName, FieldValueType.Object)
			schedulerStorage1.Appointments.CustomFieldMappings.Add(mapping)

			GenerateAppointments()

		End Sub
#Region "Appointment Generation"
		Private Sub GenerateAppointments()
			Dim now As DateTime = DateTime.Now.Date
			Dim rand As New Random()

			schedulerStorage1.BeginUpdate()
			Dim currentDate As DateTime
			For n As Integer = 0 To 4
				currentDate = now.AddDays(n)

				For i As Integer = 0 To 4
					Dim start As DateTime = currentDate.AddHours(rand.Next(24))
					Dim apt As Appointment = schedulerStorage1.CreateAppointment(AppointmentType.Normal)
					apt.Start = start
					apt.Duration = TimeSpan.FromHours(4)
					apt.Subject = String.Format("Appointment {0}{1}", n, i)


					apt.CustomFields(CustomFieldName) = CreateCustomObject(CustomFieldName, rand.Next(2))
					schedulerStorage1.Appointments.Add(apt)
				Next i
			Next n
			schedulerStorage1.EndUpdate()
		End Sub

		Private objectInfos() As String = { "green_diamond.gif", "mccarran.gif" }

		Private Function CreateCustomObject(ByVal name As String, ByVal index As Integer) As CustomObject
			Dim obj As New CustomObject()
			obj.Name = name
			obj.Info = objectInfos(index)
			obj.Picture = Image.FromFile(objectInfos(index))
			Return obj
		End Function
		#End Region

#Region "Export"
		Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
			Dim fileDialog As New SaveFileDialog()
			fileDialog.Filter = "iCalendar files (*.ics)|*.ics"
			fileDialog.FilterIndex = 1
			If fileDialog.ShowDialog() <> System.Windows.Forms.DialogResult.OK Then
				Return
			End If
			Try
					DoiCalendarExport(fileDialog.FileName)
			Catch
				MessageBox.Show("Could not export appointments","Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			End Try

		End Sub

		Private Sub DoiCalendarExport(ByVal outFileName As String)
			Dim now As DateTime = DateTime.Now.Date

			' Exporting 3 days only.
			Dim appointments As AppointmentBaseCollection = schedulerStorage1.GetAppointments(New TimeInterval(now, now.AddDays(3)))

			Dim exporter As New iCalendarExporter(schedulerStorage1, appointments)
			exporter.CustomPropertyIndentifier = CustomPropertySignature
			AddHandler exporter.AppointmentExporting, AddressOf OnExportAppointment
			Using fs As New FileStream(outFileName, FileMode.Create)
				Try
					exporter.Export(fs)
					iCalendarFileName = outFileName
				Catch e As Exception
					MessageBox.Show(e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
					iCalendarFileName = ""
				End Try
			End Using
		End Sub

		Private Sub OnExportAppointment(ByVal sender As Object, ByVal e As AppointmentExportingEventArgs)
			Dim args As iCalendarAppointmentExportingEventArgs = CType(e, iCalendarAppointmentExportingEventArgs)
			Dim apt As Appointment = args.Appointment

			' TO DO: Check whether the appointment being exported meets conditions.
			' Export appointments starting in the work time interval only.

			If apt.Start.Hour < 8 OrElse apt.Start.Hour > 17 Then
				args.Cancel = True
				Return
			End If


		End Sub
#End Region

#Region "Import"
		Private Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
			DoiCalendarImport()
		End Sub

		Private Sub DoiCalendarImport()
			Dim importer As New iCalendarImporter(schedulerStorage1)
			AddHandler importer.AppointmentImporting, AddressOf OnImportAppointment
			importer.CustomPropertyIndentifier = CustomPropertySignature
			Using fs As New FileStream(iCalendarFileName, FileMode.Open)
				Try
					importer.Import(fs)
				Catch e As Exception
					MessageBox.Show(e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
				End Try
			End Using
		End Sub

		Private Sub OnImportAppointment(ByVal sender As Object, ByVal e As AppointmentImportingEventArgs)
			Dim args As iCalendarAppointmentImportingEventArgs = CType(e, iCalendarAppointmentImportingEventArgs)
			Dim apt As Appointment = args.Appointment

			' TO DO: Check whether the event being imported meets conditions.

		End Sub
#End Region

		Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
			schedulerStorage1.BeginUpdate()
			schedulerStorage1.Appointments.Clear()
			schedulerStorage1.EndUpdate()
		End Sub

#Region "Appointment Appearance"
		Private Sub schedulerControl1_InitAppointmentDisplayText(ByVal sender As Object, ByVal e As AppointmentDisplayTextEventArgs) Handles schedulerControl1.InitAppointmentDisplayText
			Dim obj As CustomObject = TryCast(e.Appointment.CustomFields(CustomFieldName), CustomObject)
			If (obj IsNot Nothing) Then
				e.Description = obj.ToString()
			Else
				e.Description = "(no custom info)"
			End If

		End Sub
		Private Sub schedulerControl1_InitAppointmentImages(ByVal sender As Object, ByVal e As AppointmentImagesEventArgs) Handles schedulerControl1.InitAppointmentImages
			Dim c As AppointmentImageInfoCollection = e.ImageInfoList

			Dim obj As CustomObject = TryCast(e.Appointment.CustomFields(CustomFieldName), CustomObject)

			Dim info As New AppointmentImageInfo()
			If (obj IsNot Nothing) AndAlso (obj.Picture IsNot Nothing) Then
				info.Image = obj.Picture
				info.ImageIndex = 2
				c.Add(info)
			End If
		End Sub
#End Region

	End Class
End Namespace