# How to synchronize GridControl selection and filters with SchedulerControl


<p>This example describes how to synchronize GridControl selection and filters with SchedulerControl if the grid represents the source scheduler data. <br>Selection synchronization can be achieved by binding the <a href="https://documentation.devexpress.com/#WPF/DevExpressXpfSchedulerSchedulerControl_SelectedAppointmentsBindabletopic">SchedulerControl.SelectedAppointmentsBindable</a> and <a href="https://documentation.devexpress.com/#WPF/DevExpressXpfGridDataControlBase_SelectedItemstopic">GridControl.SelectedItems</a> properties to the view model properties and conversions between them. Also, it's possible to bind the <a href="https://documentation.devexpress.com/#WPF/DevExpressXpfGridDataControlBase_CurrentItemtopic">CurrentItem</a> or <a href="https://documentation.devexpress.com/WPF/DevExpressXpfGridDataControlBase_SelectedItemtopic.aspx">SelectedItem</a> property of the GridControl to update the focused grid row according to the appointment selection.<br>To apply grid control filters to the scheduler data, pass the <a href="https://documentation.devexpress.com/#WPF/DevExpressXpfGridDataControlBase_FilterChangedtopic">GridControl.FilterChanged</a> event to the view model command and set the AppointmentStorage.Filter property on the command execution correspondingly. The example also illustrates how to show an appointment edit form on the grid row's double click. </p>

<br/>


