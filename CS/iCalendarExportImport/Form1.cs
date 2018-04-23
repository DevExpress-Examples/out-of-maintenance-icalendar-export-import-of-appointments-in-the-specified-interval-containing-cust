using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.iCalendar;
using DevExpress.XtraScheduler.Drawing;


namespace iCalendarExportImport {
    public partial class Form1 : Form {
        const string CustomFieldName = "MyCustomField";
        const string CustomPropertySignature = "JohnDoeInc";
        public string iCalendarFileName = "";

      
        
        public Form1() {
            InitializeComponent();

            schedulerControl1.Start = DateTime.Now;
            schedulerControl1.DayView.DayCount = 5;

            AppointmentCustomFieldMapping mapping = new AppointmentCustomFieldMapping(CustomFieldName, CustomFieldName, FieldValueType.Object);
            schedulerStorage1.Appointments.CustomFieldMappings.Add(mapping);

            GenerateAppointments();

        }
#region Appointment Generation
        void GenerateAppointments() {
            DateTime now = DateTime.Now.Date;
            Random rand = new Random();

            schedulerStorage1.BeginUpdate();
            DateTime currentDate;
            for (int n = 0; n < 5; n++) {
                currentDate = now.AddDays(n);

                for (int i = 0; i < 5; i++) {
                    DateTime start = currentDate.AddHours(rand.Next(24));
                    Appointment apt = schedulerStorage1.CreateAppointment(AppointmentType.Normal);
                    apt.Start = start;
                    apt.Duration = TimeSpan.FromHours(4);
                    apt.Subject = String.Format("Appointment {0}{1}", n, i);


                    apt.CustomFields[CustomFieldName] = CreateCustomObject(CustomFieldName, rand.Next(2));
                    schedulerStorage1.Appointments.Add(apt);
                }
            }
            schedulerStorage1.EndUpdate();
        }

        string[] objectInfos = new string[2] { "green_diamond.gif", "mccarran.gif" };

        CustomObject CreateCustomObject(string name, int index) {
            CustomObject obj = new CustomObject();
            obj.Name = name;
            obj.Info = objectInfos[index];
            obj.Picture = Image.FromFile(objectInfos[index]);
            return obj;
        }
        #endregion
        
#region Export
        private void btnExport_Click(object sender, System.EventArgs e) {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "iCalendar files (*.ics)|*.ics";
            fileDialog.FilterIndex = 1;
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            try {
                    DoiCalendarExport(fileDialog.FileName);
            }
            catch {
                MessageBox.Show("Could not export appointments","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        void DoiCalendarExport(string outFileName) {
            DateTime now = DateTime.Now.Date;

            // Exporting 3 days only.
            AppointmentBaseCollection appointments = schedulerStorage1.GetAppointments(new TimeInterval(now, now.AddDays(3)));

            iCalendarExporter exporter = new iCalendarExporter(schedulerStorage1, appointments);
            exporter.CustomPropertyIdentifier = CustomPropertySignature;
            exporter.AppointmentExporting += new AppointmentExportingEventHandler(OnExportAppointment);
            using (FileStream fs = new FileStream(outFileName, FileMode.Create)) {
                try {
                    exporter.Export(fs);
                    iCalendarFileName = outFileName;
                }
                catch (Exception e) {
                    MessageBox.Show(e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    iCalendarFileName = "";
                }
            }
        }

        void OnExportAppointment(object sender, AppointmentExportingEventArgs e) {
            iCalendarAppointmentExportingEventArgs args = (iCalendarAppointmentExportingEventArgs)e;
            Appointment apt = args.Appointment;

            // TO DO: Check whether the appointment being exported meets conditions.
            // Export appointments starting in the work time interval only.
            
            if (apt.Start.Hour < 8 || apt.Start.Hour > 17) {
                args.Cancel = true;
                return;
            }
            

        }
#endregion
        
#region Import
        private void btnImport_Click(object sender, System.EventArgs e) {
            DoiCalendarImport();
        }

        void DoiCalendarImport() {
            iCalendarImporter importer = new iCalendarImporter(schedulerStorage1);
            importer.AppointmentImporting += new AppointmentImportingEventHandler(OnImportAppointment);
            importer.CustomPropertyIdentifier = CustomPropertySignature;
            using (FileStream fs = new FileStream(iCalendarFileName, FileMode.Open)) {
                try {
                    importer.Import(fs);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void OnImportAppointment(object sender, AppointmentImportingEventArgs e) {
            iCalendarAppointmentImportingEventArgs args = (iCalendarAppointmentImportingEventArgs)e;
            Appointment apt = args.Appointment;
            
            // TO DO: Check whether the event being imported meets conditions.

        }
#endregion

        private void btnClear_Click(object sender, System.EventArgs e) {
            schedulerStorage1.BeginUpdate();
            schedulerStorage1.Appointments.Clear();
            schedulerStorage1.EndUpdate();
        }

#region Appointment Appearance
        private void schedulerControl1_InitAppointmentDisplayText(object sender, AppointmentDisplayTextEventArgs e) {
            CustomObject obj = e.Appointment.CustomFields[CustomFieldName] as CustomObject;
            e.Description = (obj != null) ? obj.ToString() : "(no custom info)";

        }
        private void schedulerControl1_InitAppointmentImages(object sender, AppointmentImagesEventArgs e) {
            AppointmentImageInfoCollection c = e.ImageInfoList;

            CustomObject obj = e.Appointment.CustomFields[CustomFieldName] as CustomObject;

            AppointmentImageInfo info = new AppointmentImageInfo();
            if ((obj != null) && (obj.Picture != null)) {
                info.Image = obj.Picture;
                info.ImageIndex = 2;
                c.Add(info);
            }
        }
#endregion

    }
}