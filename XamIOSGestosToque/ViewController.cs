using System;
using CoreGraphics;
using UIKit;

namespace XamIOSGestosToque
{
    public partial class ViewController : UIViewController
    {
        nfloat rotacion = 0;
        nfloat coordenadaX = 0;
        nfloat coordenadaY = 0;
        bool Toque;

        UIRotationGestureRecognizer GestoRotar;
        UIPanGestureRecognizer GestoMover;
        UIAlertController Alerta;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            Imagen.UserInteractionEnabled = true;
            var GestoToque = new UITapGestureRecognizer(Tocando);
            Imagen.AddGestureRecognizer(GestoToque);

            GestoMover = new UIPanGestureRecognizer(() =>
            {

                if ((GestoMover.State == UIGestureRecognizerState.Began ||
                    GestoMover.State == UIGestureRecognizerState.Changed)
                              && (GestoMover.NumberOfTouches == 1))
                {
                    var p0 = GestoMover.LocationInView(View);
                    if (coordenadaX == 0)
                        coordenadaX = p0.X - Imagen.Center.X;
                    if (coordenadaY == 0)
                        coordenadaY = p0.Y - Imagen.Center.Y;
                    var p1 = new CGPoint(p0.X - coordenadaX, p0.Y - coordenadaY);
                    Imagen.Center = p1;
                }
                else
                {
                    coordenadaX = 0;
                    coordenadaY = 0;

                }
            });

            GestoRotar = new UIRotationGestureRecognizer(()=>
            {
                if((GestoRotar.State == UIGestureRecognizerState.Began ||
                    GestoRotar.State == UIGestureRecognizerState.Changed)
                    && (GestoRotar.NumberOfTouches == 2))
                {
                    Imagen.Transform = CGAffineTransform.MakeRotation(GestoRotar.Rotation + rotacion);

                }else if(GestoRotar.State == UIGestureRecognizerState.Ended)
                {
                    rotacion = GestoRotar.Rotation;
                }
            });

            Imagen.AddGestureRecognizer(GestoMover);
            Imagen.AddGestureRecognizer(GestoRotar);
        }

        private void Tocando(UITapGestureRecognizer toque)
        {
            // throw new NotImplementedException();
            if(!Toque)
            {
                toque.View.Transform *= CGAffineTransform.MakeRotation((float)
                                                                       Math.PI);
                Toque = true;
                Alerta = UIAlertController.Create("Imagen Tocada", "Imagen Girando", 
                                                  UIAlertControllerStyle.Alert);
                Alerta.AddAction(UIAlertAction.Create("Aceptar",
                                                      UIAlertActionStyle.Default, null));
                PresentViewController(Alerta,true,null);

            }else{

                toque.View.Transform *= CGAffineTransform.MakeRotation((float)
                                                                      -Math.PI);
                Toque = false;
                Alerta = UIAlertController.Create("Imagen Regresando", "Imagen Regresando",
                                                  UIAlertControllerStyle.Alert);
                Alerta.AddAction(UIAlertAction.Create("Aceptar",
                                                      UIAlertActionStyle.Default, null));
                PresentViewController(Alerta, true, null);

            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
