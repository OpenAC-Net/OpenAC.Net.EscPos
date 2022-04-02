namespace OpenAC.Net.EscPos.Demo.Commom;

public static class UIExtensions
{
    public static void EnumDataSource<T>(this ComboBox cmb) where T : struct
    {
        cmb.DataSource = (from T value in Enum.GetValues(typeof(T)) select new ItemData<T>(value)).ToArray();
    }

    public static void EnumDataSource<T>(this ComboBox cmb, T valorPadrao) where T : struct
    {
        var list = (from T value in Enum.GetValues(typeof(T)) select new ItemData<T>(value.ToString(), value)).ToArray();
        cmb.DataSource = list;
        cmb.SelectedItem = list.SingleOrDefault(x => x.Content.Equals(valorPadrao));
    }

    public static void SetDataSource<T>(this ComboBox cmb, IEnumerable<T> valores)
    {
        var list = (from T value in valores select new ItemData<T>(value.ToString(), value)).ToArray();
        cmb.DataSource = list;
        cmb.SelectedItem = list.First();
    }

    public static T GetSelectedValue<T>(this ComboBox cmb)
    {
        return ((ItemData<T>)cmb.SelectedItem).Content;
    }

    public static void SetSelectedValue<T>(this ComboBox cmb, T valor)
    {
        var dataSource = (ItemData<T>[])cmb.DataSource;
        cmb.SelectedItem = dataSource.SingleOrDefault(x => x.Content.Equals(valor));
    }

    public static void HideTabHeaders(this TabControl tabControl)
    {
        tabControl.Appearance = TabAppearance.FlatButtons;
        tabControl.ItemSize = new Size(0, 1);
        tabControl.SizeMode = TabSizeMode.Fixed;

        tabControl.SelectedTab = tabControl.TabPages[0];
    }
}