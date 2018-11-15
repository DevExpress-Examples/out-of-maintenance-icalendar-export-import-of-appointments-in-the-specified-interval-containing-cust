<!-- default file list -->
*Files to look at*:

* [CustomObject.cs](./CS/iCalendarExportImport/CustomObject.cs) (VB: [CustomObject.vb](./VB/iCalendarExportImport/CustomObject.vb))
* [Form1.cs](./CS/iCalendarExportImport/Form1.cs) (VB: [Form1.vb](./VB/iCalendarExportImport/Form1.vb))
<!-- default file list end -->
# iCalendar export/import of appointments in the specified interval containing custom objects


<p>This example illustrates how you can apply criteria to export only specific appointments in the iCalendar format. The <a href="http://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraScheduleriCalendariCalendarExportertopic">iCalendarExporter</a> and <a href="http://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraScheduleriCalendariCalendarImportertopic">iCalendarImporter</a> classes are used to perform this task.</p><p>The appointments for export are obtained from the time interval which is three days long only. In the <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerExchangeAppointmentImporter_AppointmentImportingtopic">AppointmentImporting</a> event handler all appointments which start beyond the working time (8 AM - 5 PM) are skipped.</p><p>The appointment has a custom field which contain a custom object. This object is serialized on export and written as a non-standard property value to the iCalendar file. When appointment is imported, the object is restored to the custom field. This process is performed silently by the appointment importer and exporter objects. You should specify a Serializable attribute to the custom class to make use of this feature.</p>

<br/>


