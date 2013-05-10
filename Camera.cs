using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LudumDareGame
{
    
    class Camera
    {
        #region Local variables
            private float zoom;
            public Vector2 camera2DPos;
            private Matrix transformMatrix;

        #endregion

        #region constructor
        public Camera(Vector2 cameraPos)
        {
            this.zoom = 2f;   
            this.camera2DPos = cameraPos;
        }
        #endregion

        #region Local functions
        
        public float Zoom(float zoomValue)
        {
            this.zoom = zoomValue;
            if (this.zoom <= 0.1f)
            {
                this.zoom = 2.0f;
            }

            return this.zoom;
        }
        public void UnZoom()
        {
            this.zoom -= 0.008f; 
        }
        public void Zoom()
        {
            this.zoom += 0.008f;
        }
        public void MoveCamera(Vector2 moveAmount)
        {
            this.camera2DPos += moveAmount;
        }
        

        public Matrix transform_get(GraphicsDeviceManager graphics)
        {
          
                transformMatrix =
                  Matrix.CreateTranslation(new Vector3(-camera2DPos.X, -camera2DPos.Y, 0)) *
                  Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                  Matrix.CreateTranslation(new Vector3(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight * 0.5f, 0));

                return transformMatrix;

        }

        #endregion

    }
}
