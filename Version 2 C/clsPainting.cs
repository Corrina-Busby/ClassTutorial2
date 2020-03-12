using System;
/// <summary>
/// Singleton and Obsever Pattern 
/// </summary>
namespace Version_2_C
{
    [Serializable()]
    public class clsPainting : clsWork
    {
        // Applying the Obsever pattern to avoid dependance on frmPainting due to frequent GUI changes 
        public delegate void LoadPaintingFormDelegate(clsPainting prPainting);
        public static LoadPaintingFormDelegate LoadPaintingForm;

        private float _Width;
        private float _Height;
        private string _Type;

        //[NonSerialized()]
        //private static frmPainting _PaintDialog;
        public override void EditDetails()
        {
            LoadPaintingForm(this);
            //if (frmPainting.Instance == null)
             //  frmPainting.Instance = new frmPainting.Instance();
            //frmPainting.Instance.SetDetails(this);
        }

        public Single Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        public Single Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
    }
}
