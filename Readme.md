<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128657504/15.1.8%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T328445)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [DataProvider.cs](./CS/DataProvider.cs) (VB: [DataProvider.vb](./VB/DataProvider.vb))
* [MainWindow.xaml](./CS/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/MainWindow.xaml))
* [SchedulerModel.cs](./CS/SchedulerModel.cs) (VB: [SchedulerModel.vb](./VB/SchedulerModel.vb))
* **[SchedulerViewModel.cs](./CS/SchedulerViewModel.cs) (VB: [SchedulerViewModel.vb](./VB/SchedulerViewModel.vb))**
<!-- default file list end -->
# How to synchronize GridControl selection and filters with SchedulerControl


<p>This example describes how to synchronize GridControl selection and filters with SchedulerControl if the grid represents the source scheduler data. <br>Selection synchronization can be achieved by binding the <a href="https://documentation.devexpress.com/#WPF/DevExpressXpfSchedulerSchedulerControl_SelectedAppointmentsBindabletopic">SchedulerControl.SelectedAppointmentsBindable</a> and <a href="https://documentation.devexpress.com/#WPF/DevExpressXpfGridDataControlBase_SelectedItemstopic">GridControl.SelectedItems</a> properties to the view model properties and conversions between them. Also, it's possible to bind the <a href="https://documentation.devexpress.com/#WPF/DevExpressXpfGridDataControlBase_CurrentItemtopic">CurrentItem</a> or <a href="https://documentation.devexpress.com/WPF/DevExpressXpfGridDataControlBase_SelectedItemtopic.aspx">SelectedItem</a> property of the GridControl to update the focused grid row according to the appointment selection.<br>To apply grid control filters to the scheduler data, pass the <a href="https://documentation.devexpress.com/#WPF/DevExpressXpfGridDataControlBase_FilterChangedtopic">GridControl.FilterChanged</a> event to the view model command and set the AppointmentStorage.Filter property on the command execution correspondingly. The example also illustrates how to show an appointment edit form on the grid row's double click. </p>

<br/>


