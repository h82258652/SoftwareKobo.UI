using Windows.UI;

namespace SoftwareKobo.UI
{
    public interface IHasShadow
    {
        double BlurRadius
        {
            get;
            set;
        }

        Color Color
        {
            get;
            set;
        }

        double Depth
        {
            get;
            set;
        }

        double Direction
        {
            get;
            set;
        }

        double ShadowOpacity
        {
            get;
            set;
        }
    }
}