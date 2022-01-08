using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MedicalLotModel;
using System.Data.Entity;
using System.Data;

namespace Proiect_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        MedicalLotEntitiesModel ptx = new MedicalLotEntitiesModel();
        CollectionViewSource patientVSource, doctorVSource;
        CollectionViewSource patientAppointmentsVSource;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            patientVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("patientViewSource")));
            patientVSource.Source = ptx.Patients.Local;
            ptx.Patients.Load();
            // Load data by setting the CollectionViewSource.Source property:
            // patientViewSource.Source = [generic data source]

            doctorVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("doctorViewSource")));
            doctorVSource.Source = ptx.Doctors.Local;
            ptx.Doctors.Load();
            // Load data by setting the CollectionViewSource.Source property:
            // doctorViewSource.Source = [generic data source]
            patientAppointmentsVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("patientAppointmentsViewSource")));
            //patientAppointmentsVSource.Source = ptx.Appointments.Local;
            ptx.Appointments.Load();

            ptx.Doctors.Load();

            cmbPatients.ItemsSource = ptx.Patients.Local;
            //cmbPatients.DisplayMemberPath = "PatientFirstName";
           // cmbPatients.DisplayMemberPath = "PatientLastName";
            cmbPatients.SelectedValuePath = "PatientId";
            cmbDoctors.ItemsSource = ptx.Doctors.Local;
            //cmbDoctors.DisplayMemberPath = "DoctorFirstName";
            //cmbDoctors.DisplayMemberPath = "DoctorLastName";
            cmbDoctors.SelectedValuePath = "DoctorId";

            BindDataGrid();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            SetValidationBinding();
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(patientFirstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(patientLastNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(doctorFirstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(doctorLastNameTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            patientVSource.View.MoveCurrentToNext();
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            patientVSource.View.MoveCurrentToPrevious();
        }

        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            doctorVSource.View.MoveCurrentToNext();
        }
        private void btnPrevious1_Click(object sender, RoutedEventArgs e)
        {
            doctorVSource.View.MoveCurrentToPrevious();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReInitialize();
        }

        private void SavePatients()
        {
            Patient patient = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Patient entity
                    patient = new Patient()
                    {
                        PatientFirstName = patientFirstNameTextBox.Text.Trim(),
                        PatientLastName = patientLastNameTextBox.Text.Trim(),
                        CNP = Convert.ToInt32(this.cNPTextBox.Text.Trim()),
                        Disease = diseaseTextBox.Text.Trim(),

                    };
                    //adaugam entitatea nou creata in context
                    ptx.Patients.Add(patient);
                    patientVSource.View.Refresh();
                    //salvam modificarile
                    ptx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    patient = (Patient)patientDataGrid.SelectedItem;
                    patient.PatientFirstName = patientFirstNameTextBox.Text.Trim();
                    patient.PatientLastName = patientLastNameTextBox.Text.Trim();
                    patient.CNP = Convert.ToInt32(this.cNPTextBox.Text.Trim());
                    patient.Disease = diseaseTextBox.Text.Trim();
                    //salvam modificarile
                    ptx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    patient = (Patient)patientDataGrid.SelectedItem;
                    ptx.Patients.Remove(patient);
                    ptx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                patientVSource.View.Refresh();
            }

        }

        private void SaveDoctors()
        {
            Doctor doctor = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    doctor = new Doctor()
                    {
                        DoctorFirstName = doctorFirstNameTextBox.Text.Trim(),
                        DoctorLastName = doctorLastNameTextBox.Text.Trim(),
                        Rank = rankTextBox.Text.Trim(),
                        Specialization = specializationTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ptx.Doctors.Add(doctor);
                    doctorVSource.View.Refresh();
                    //salvam modificarile
                    ptx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    doctor = (Doctor)doctorDataGrid.SelectedItem;
                    doctor.DoctorFirstName = doctorFirstNameTextBox.Text.Trim();
                    doctor.DoctorLastName = doctorLastNameTextBox.Text.Trim();
                    doctor.Rank = rankTextBox.Text.Trim();
                    doctor.Specialization = specializationTextBox.Text.Trim();
                    //salvam modificarile
                    ptx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    doctor = (Doctor)doctorDataGrid.SelectedItem;
                    ptx.Doctors.Remove(doctor);
                    ptx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                doctorVSource.View.Refresh();
            }

        }

        private void gbOperations_Click(object sender, RoutedEventArgs e)
        {
            Button SelectedButton = (Button)e.OriginalSource;
            Panel panel = (Panel)SelectedButton.Parent;

            foreach (Button B in panel.Children.OfType<Button>())
            {
                if (B != SelectedButton)
                    B.IsEnabled = false;
            }
            gbActions.IsEnabled = true;
        }

        private void ReInitialize()
        {

            Panel panel = gbOperations.Content as Panel;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
            gbActions.IsEnabled = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = tbCtrlMedicalLot.SelectedItem as TabItem;

            switch (ti.Header)
            {
                case "Patients":
                    SavePatients();
                    break;
                case "Doctors":
                    SaveDoctors();
                    break;
                case "Appointments":
                    SaveAppointments();
                    break;
            }
            ReInitialize();
        }
        private void SaveAppointments()
        {
            Appointment appointment = null;
            if (action == ActionState.New)
            {
                try
                {
                    Patient patient = (Patient)cmbPatients.SelectedItem;
                    Doctor doctor = (Doctor)cmbDoctors.SelectedItem;
                    //instantiem Appointment entity
                    appointment = new Appointment()
                    {

                        PatientId = patient.PatientId,
                        DoctorId = doctor.DoctorId
                    };
                    //adaugam entitatea nou creata in context
                    ptx.Appointments.Add(appointment);
                    //salvam modificarile
                    ptx.SaveChanges();
                    BindDataGrid();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                dynamic selectedAppointment = appointmentsDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedAppointment.AppointmentId;
                    var editedAppointment = ptx.Appointments.FirstOrDefault(s => s.AppointmentId == curr_id);
                    if (editedAppointment != null)
                    {
                        editedAppointment.PatientId = Int32.Parse(cmbPatients.SelectedValue.ToString());
                        editedAppointment.DoctorId = Convert.ToInt32(cmbDoctors.SelectedValue.ToString());
                        //salvam modificarile
                        ptx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                patientAppointmentsVSource.View.MoveCurrentTo(selectedAppointment);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedAppointment = appointmentsDataGrid.SelectedItem;
                    int curr_id = selectedAppointment.AppointmentId;
                    var deletedAppointment = ptx.Appointments.FirstOrDefault(s => s.AppointmentId == curr_id);
                    if (deletedAppointment != null)
                    {
                        ptx.Appointments.Remove(deletedAppointment);
                        ptx.SaveChanges();
                        MessageBox.Show("Appointment Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BindDataGrid()
        {
            var queryOrder = from app in ptx.Appointments
                             join pat in ptx.Patients on app.PatientId equals
                             pat.PatientId
                             join doc in ptx.Doctors on app.DoctorId
                 equals doc.DoctorId
                             select new
                             {
                                 app.AppointmentId,
                                 app.DoctorId,
                                 app.PatientId,
                                 pat.PatientFirstName,
                                 pat.PatientLastName,
                                 doc.DoctorFirstName,
                                 doc.DoctorLastName,
                                 pat.Disease,
                                 app.AppDate
                             };
            patientAppointmentsVSource.Source = queryOrder.ToList();
        }

        private void SetValidationBinding()
        {
            Binding patientfirstNameValidationBinding = new Binding();
            patientfirstNameValidationBinding.Source = patientVSource;
            patientfirstNameValidationBinding.Path = new PropertyPath("PatientFirstName");
            patientfirstNameValidationBinding.NotifyOnValidationError = true;
            patientfirstNameValidationBinding.Mode = BindingMode.TwoWay;
            patientfirstNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string required
            patientfirstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            patientFirstNameTextBox.SetBinding(TextBox.TextProperty,
           patientfirstNameValidationBinding);


            Binding doctorfirstNameValidationBinding = new Binding();
            doctorfirstNameValidationBinding.Source = doctorVSource;
            doctorfirstNameValidationBinding.Path = new PropertyPath("DoctorFirstName");
            doctorfirstNameValidationBinding.NotifyOnValidationError = true;
            doctorfirstNameValidationBinding.Mode = BindingMode.TwoWay;
            doctorfirstNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string required
            patientfirstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            doctorFirstNameTextBox.SetBinding(TextBox.TextProperty,
           doctorfirstNameValidationBinding);
            doctorFirstNameTextBox.SetBinding(TextBox.TextProperty, patientfirstNameValidationBinding);
            
            Binding patientlastNameValidationBinding = new Binding();
            patientlastNameValidationBinding.Source = patientVSource;
            patientlastNameValidationBinding.Path = new PropertyPath("PatientLastName");
            patientlastNameValidationBinding.NotifyOnValidationError = true;
            patientlastNameValidationBinding.Mode = BindingMode.TwoWay;
            patientlastNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            patientlastNameValidationBinding.ValidationRules.Add(new
           StringMinLengthValidator());
            patientLastNameTextBox.SetBinding(TextBox.TextProperty,
           patientlastNameValidationBinding); //setare binding nou

            Binding doctorlastNameValidationBinding = new Binding();
            doctorlastNameValidationBinding.Source = patientVSource;
            doctorlastNameValidationBinding.Path = new PropertyPath("DoctorLastName");
            doctorlastNameValidationBinding.NotifyOnValidationError = true;
            doctorlastNameValidationBinding.Mode = BindingMode.TwoWay;
            doctorlastNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            doctorlastNameValidationBinding.ValidationRules.Add(new
           StringMinLengthValidator());
            doctorLastNameTextBox.SetBinding(TextBox.TextProperty,
           doctorlastNameValidationBinding); //setare binding nou

       
    } }
}