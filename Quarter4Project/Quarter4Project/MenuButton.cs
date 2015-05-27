using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class MenuButton : AnimatedSprite
    {

        public enum ButtonAction { PLAY, SAVE, LOAD, OPTIONS, CREDITS, MENU };

        public ButtonAction action;
        MenuManager myMenu;
        MouseState mouse, mousePrev;
        SpriteFont font;
        String words;
        Vector2 printOffset;


        public MenuButton(Vector2 v, ButtonAction a, Texture2D t, string s, SpriteFont f, MenuManager m) : base(t)
        {
            position = v;
            action = a;
            myMenu = m;
            font = f;
            words = s;
            printOffset = font.MeasureString(words);
            printOffset.X = (300 / 2) - (printOffset.X / 2);
            printOffset.Y = (82 / 2) - (printOffset.Y / 2);
            mouse = mousePrev = Mouse.GetState();
            addAnimations(); 
        }

        public override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            if(Collision.CheckPointRectangleCollision(Collision.toPoint(myMenu.cursor.getPos()), collisionRect()))
            {
                if(mouse.LeftButton == ButtonState.Pressed)
                {
                    setAnimation("CLICKED");
                }
                else
                {
                    setAnimation("HOVERED");
                    if(mousePrev.LeftButton == ButtonState.Pressed)
                    {
                        runAction();
                    }
                }
            }
            else
            {
                setAnimation("IDLE");
            }

            base.Update(gameTime);
                mousePrev = mouse;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                spriteBatch.Draw(textures[i], position, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i]);
            }
            spriteBatch.DrawString(font, words, position + printOffset, Color.Black);
        }

        public override void addAnimations()
        {
            sets.Add(new AnimationSet("IDLE", new Point(300, 82), new Point(1, 1), new Point(0, 0), 0, false));
            sets.Add(new AnimationSet("HOVERED", new Point(300, 82), new Point(1, 1), new Point(0, 1), 0, false));
            sets.Add(new AnimationSet("CLICKED", new Point(300, 82), new Point(1, 1), new Point(0, 2), 0, false));
        }

        public void runAction()
        {
            switch(action)
            {
                case ButtonAction.CREDITS:
                    testPurple();
                    break;
                case ButtonAction.LOAD:
                    testPurple();
                    break;
                case ButtonAction.MENU:
                    testPurple();
                    break;
                case ButtonAction.OPTIONS:
                    testPurple();
                    break;
                case ButtonAction.PLAY:
                    myMenu.myGame.SetCurrentLevel(Game1.GameLevels.PLAY);
                    break;
                case ButtonAction.SAVE:
                    testPurple();
                    break;
            }
        }

        void testPurple()
        {      
            colors[0] = Color.Purple;
        }

    }
}
