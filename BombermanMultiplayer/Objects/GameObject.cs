using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Media;
using System.Diagnostics;

namespace BombermanMultiplayer
{
    [Serializable]
    public abstract class GameObject
    {
        //Rectangle permettant de 'matérialiser' le sprite
        protected Rectangle _Source;

        [NonSerialized]
        protected Image Sprite;

        //Frame actuelle de l'objet
        protected int _frameindex;
        //Durée depuis l'affichage d'une frame

        //Durée d'une frame
        protected float _frameTime = 125;


        protected int _totalElapsedTime = 0;

        //Nombre de frame de l'animation
        protected int _totalFrames;

        //Position de l'objet au niveau case
        protected int[] _CasePosition;

        #region Accessors


        public int[] CasePosition
         {
             get { return _CasePosition; }
             set
             {
                if (!(value[0] < 0 || value[1] < 0))
                {
                    _CasePosition = value;

                }
            }

         }
        public Rectangle Source
        {
            get { return _Source; }
            set { _Source = value; }
        }
        public float frameSpeed
        {
            get { return this._frameTime; }
            set
            {
                if (value <= 0) this._frameTime = 1; //Pas de temps négatif
                else this._frameTime = value;
            }
        }

        public int totalFrames
        {
            get { return _totalFrames; }
        }
        public int frameindex
        {
            get { return _frameindex; }
            set { _frameindex = value; }
        }


        #endregion

        //Constructeur
        public GameObject()
        { }

        public GameObject(int x, int y, int totalFrames, int frameWidth, int frameHeight)
            
        {
            _totalFrames = totalFrames;
            CasePosition = new int[2] { 0, 0 };
            _Source = new Rectangle(x, y, frameWidth, frameHeight);
            
        }

        public GameObject(int x, int y, int totalFrames, int frameWidth, int frameHeight, int frameTime)

        {
            _totalFrames = totalFrames;
            CasePosition = new int[2] { 0, 0 };
            _Source = new Rectangle(x, y, frameWidth, frameHeight);
            _frameTime = frameTime;
            

        }

        public void ChangeLocation(int x, int y)
        {
            this._Source.X = x;
            this._Source.Y = y;
        }

        
        public void LoadSprite(Image sprite)
        {

            this.Sprite = sprite;

        }
        public void UnloadSprite()
        {

            this.Sprite = null;

        }


        
        public void Draw(Graphics gr)
        {
            if (this.Sprite != null)
            {
                gr.DrawImage(this.Sprite, Source, frameindex * Source.Width, 0, Source.Width, Source.Height, GraphicsUnit.Pixel);
                gr.DrawRectangle(Pens.Red, this.Source);
            }
        }




        public void UpdateFrame(int elsapedTime)
        {

            _totalElapsedTime += elsapedTime;

            if (_totalElapsedTime > this.frameSpeed)
            {
                frameindex += 1;

                _totalElapsedTime = 0;

                if (frameindex > _totalFrames)
                {
                    frameindex = 0; // retourne au premier sprite une fois la série finie

                }
            }


        }



        public void Bouger(int deplX, int deplY) // Ajoute juste le montant du déplacement à la postion de l'objet.
        {
            _Source.X += deplX;
            _Source.Y += deplY;
            
        }

        


    }
}
