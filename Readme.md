<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128636483/13.1.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E547)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [CustomObject.cs](./CS/iCalendarExportImport/CustomObject.cs) (VB: [CustomObject.vb](./VB/iCalendarExportImport/CustomObject.vb))
* [Form1.cs](./CS/iCalendarExportImport/Form1.cs) (VB: [Form1.vb](./VB/iCalendarExportImport/Form1.vb))
* [Program.cs](./CS/iCalendarExportImport/Program.cs) (VB: [Program.vb](./VB/iCalendarExportImport/Program.vb))
<!-- default file list end -->
# iCalendar export/import of appointments in the specified interval containing custom objects


<p>This example illustrates how you can apply criteria to export only specific appointments in the iCalendar format. The <a href="http://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraScheduleriCalendariCalendarExportertopic">iCalendarExporter</a> and <a href="http://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraScheduleriCalendariCalendarImportertopic">iCalendarImporter</a> classes are used to perform this task.</p><p>The appointments for export are obtained from the time interval which is three days long only. In the <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerExchangeAppointmentImporter_AppointmentImportingtopic">AppointmentImporting</a> event handler all appointments which start beyond the working time (8 AM - 5 PM) are skipped.</p><p>The appointment has a custom field which contain a custom object. This object is serialized on export and written as a non-standard property value to the iCalendar file. When appointment is imported, the object is restored to the custom field. This process is performed silently by the appointment importer and exporter objects. You should specify a Serializable attribute to the custom class to make use of this feature.</p>

<br/>


