namespace GPUProject.lab1v2;

static class Extension {
    public static void SetBind(this Control control, string propertyName, object dataSource, string dataMember) {
        control.DataBindings.Clear();
        control.DataBindings.Add(propertyName, dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged);
    }
}