using System;

namespace Version_2_C
{
    [Serializable()]
    public class clsSculpture : clsWork
    {
        private float _Weight;
        private string _Material;

        //[NonSerialized()]
        //private static frmSculpture _SculptureDialog;

        public override void EditDetails()
        {
            if (frmSculpture.Instance == null)
              //  _SculptureDialog = new frmSculpture();
            frmSculpture.Instance.SetDetails(this);
        }

        public Single Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }

        public string Material
        {
            get { return _Material; }
            set { _Material = value; }
        }
    }
}
