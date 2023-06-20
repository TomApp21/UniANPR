namespace UniANPR.Models.Shared
{
      /// <summary>
    /// Class used to define content of display items requiring a display name and an underlying id
    /// </summary>
    public class ValueTextModel
    {
        public ValueTextModel()
        {
            //DdlValueField = ddlValueField;
            //DdlTextField = ddlTextField;
        }

        public object DdlValueField { get; set; }
        public string DdlTextField { get; set; }
    }
}
