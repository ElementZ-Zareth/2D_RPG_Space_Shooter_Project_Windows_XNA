using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //testing stuff

        //intialize walls
        Walls wall1;
        //Walls wall2;
        //Walls wall3;
        //Walls wall4;
        Texture2D spriteWall;
        Sprite basewall;

        Damage damage;

        //planet test
        Sprite planet;
        Sprite planet2;
        Sprite planet3;
        Texture2D planetTexture;

        // Reprsents the player
        Sprite playerSprite;
        Player player;
        Sprite playerShooter;
        Sprite playerShield;

        // GUI Declarations
        Sprite playerHPMeter;
        Sprite playerEnergyMeter;
        Sprite playerShieldMeter;
        Sprite meterBackHealth;
        Sprite meterBackShield;
        Sprite meterBackEnergy;
        Sprite bottomBar;

        Texture2D hpMeterTexture;
        Texture2D energyMeterTexture;
        Texture2D shieldMeterTexture;
        Texture2D meterBackTexture;
        Texture2D bottomBarTexture;

        // Mouse and Cursor
        Sprite mouseSprite;
        Texture2D mouseTexture;
        Cursor cursor;
        Sprite shooterAimSprite;
        Texture2D shooterAimTexture;
        int mosX, mosY;
        int CAmosX, CAmosY;

        //Initialize starfield
        StarField starField;
        Texture2D spriteSheet;

        
        // GameState
        enum GameState {SplashScreen, TitleScreen, Play, Pause, ChooseAction, GameOver};
        GameState gameState = GameState.Play;
        float screenOverLayTransparency;
        Texture2D screenOverlay;
        bool escapeDown;

        // Projectile Type
        enum ProjectileType {Straight, Scatter, Charge, Wave}
        ProjectileType projectileType = ProjectileType.Straight;
        int Angle;
        Texture2D projectileTexture;
        List<Projectile> projectiles;

        // Fire Shot Sprites
        List<Projectile> fireShotSprites;
        Texture2D fireShotTexture;
        List<Projectile> hitSprites;

        // Drop list and sprites
        List<Drops> drops;
        Texture2D experienceDropTexture;
        Texture2D healthDropTexture;

        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        // A movement speed for the player
        float playerMoveSpeed;
        float playerMoveX;
        float playerMoveY;

        // Master Sound volume
        float Sound;

        // Image used to display the static background
        Texture2D mainBackground;

        ParallaxingBackground bgLayer1;

        // Enemies
        Texture2D enemyTexture;
        List<Enemy> enemies;

        // The rate at which the enemies appear
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        // Player hit invincibility time
        TimeSpan gotHit;
        TimeSpan hitTime;
        TimeSpan previousgotHit;

        // Enemy waves
        int enemyCount;
        int wave;

        // A random number generator
        Random random;

        Texture2D eprojectileTexture;
        List<Projectile> enemyProjectiles;     

        Texture2D explosionTexture;
        List<Sprite> explosions;

        // Choose Action Icons
        bool ChooseActionActive;
        List<ChooseActionIcons> CAIcons;
        Texture2D blankIcon;
        Texture2D straightIcon;
        Texture2D scatterIcon;
        Texture2D chargeIcon;
        Texture2D waveIcon;


        // Sound Effects
        // The sound that is played when a laser is fired
        SoundEffect laserSound;
        // The sound that is played when charging
        SoundEffect chargeSound;
        // The sound used when the player or an enemy dies
        SoundEffect explosionSound;

        // The music played during gameplay
        Song gameplayMusic;

        // Number that holds the player score
        int score;
        // the font used to display UI elements
        SpriteFont font;

        // player alpha float
        float playerAlpha;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Window.Title = "Awesome!";
            
            //this.graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            bgLayer1 = new ParallaxingBackground();

            escapeDown = false;

            explosions = new List<Sprite>();

            // set player's score to zero
            score = 0;
            Sound = 0f;

            // set screen overlay
            screenOverLayTransparency = 0.0f;

            // Initialize projectiles list
            projectiles = new List<Projectile>();

            // Initialize fire shot list
            fireShotSprites = new List<Projectile>();

            // Initialize hit sprite list
            hitSprites = new List<Projectile>();

            // Initialize enemy projectile list
            enemyProjectiles = new List<Projectile>();

            // Initialize Drops list
            drops = new List<Drops>();

            // Initialize Choose Action Icon list
            CAIcons = new List<ChooseActionIcons>();
            ChooseActionActive = false;

            // Initialize the enemies list
            enemies = new List<Enemy>();

            // Reset the Enemy Spawn Time to zero
            previousSpawnTime = TimeSpan.Zero;
            
            // Used to determine how fast enemy respawns
            enemySpawnTime = TimeSpan.FromSeconds(0.35f);

            // Used to determine how long player is invincible on hit
            playerAlpha = 1.0f;
            gotHit = TimeSpan.Zero;
            previousgotHit = TimeSpan.Zero;
            hitTime = TimeSpan.FromSeconds(1.0f);

            // Start enemy wave @ 1
            wave = 1;
            enemyCount = wave * 3;

            // Initialize our random number generator

            random = new Random();

            // Initialize the player class
            player = new Player();

            // Initialize the cursor class
            cursor = new Cursor();

            // damage?
            damage = new Damage();

            // Set a constant player move speed
            playerMoveSpeed = 8.0f;
            playerMoveX = 0.0f;
            playerMoveY = 0.0f;

            // Enable the FreeDrag gesture.
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // Camera Variables
            // Note: Highest pixel Height is around 7,200,000
            Camera.WorldRectangle = new Rectangle(0, 0, 2560, 1440);
            Camera.ViewPortWidth = 1280;
            Camera.ViewPortHeight = 720;

            //Jupiter Distance
            Camera.Depth = 0d;

            // screen overlay
            screenOverlay = Content.Load<Texture2D>("screenoverlay");

            //wall testing stuff
            spriteWall = Content.Load<Texture2D>(@"SideWall");
            wall1 = new Walls(new Vector2(980, 640), 0, spriteWall, (float)Math.PI / 2);
            basewall = new Sprite(new Vector2(980, 640), 0, spriteWall, new Rectangle(0, 0, spriteWall.Width, spriteWall.Height), new Vector2(0, 0));
            basewall.layerDepth = false;


            //planet stuff
            planetTexture = Content.Load<Texture2D>("firstplanet");
            planet = new Sprite(new Vector2(0f, 0f), 4830000000d, planetTexture, new Rectangle(0, 0, 1000, 1000), Vector2.Zero);
            planet.Scale = 124411728f;
            planet.layerDepth = false;
            
            planet2 = new Sprite(new Vector2(0f, 0f), 48300000d, planetTexture, new Rectangle(0, 0, 1000, 1000), Vector2.Zero);
            planet2.Scale = 1141392f;
            planet2.layerDepth = false;
            
            planet2.TintColor = Color.Goldenrod;

            planet3 = new Sprite(new Vector2(980, 640), 48299950, planetTexture, new Rectangle(0, 0, 1000, 1000), Vector2.Zero);
            //planet3.Scale = 308175.84f;
            planet3.Scale = 3f;
            planet3.layerDepth = false;
            
            planet3.TintColor = Color.AliceBlue;

            //planet.Velocity = new Vector2(0, 999999999f);

            //starfield stuff
            spriteSheet = Content.Load<Texture2D>(@"Star");
            starField = new StarField(60000000000000f, 60000000000000f, 500, new Vector2(0, 0), spriteSheet, new Rectangle(0, 0, 10, 10));



            // Load the player resources
            Texture2D playerTexture = Content.Load<Texture2D>("AvatarSprite");
            playerSprite = new Sprite(new Vector2(0, 0), 1d, playerTexture, new Rectangle(0, 0, 100, 150), Vector2.Zero);
            Vector2 playerPosition = new Vector2(1280, 720);
            player.Initialize(playerSprite, playerPosition);
            playerSprite.setDepth = false;
            
            Texture2D playerShieldTexture = Content.Load<Texture2D>("Shield");
            playerShield = new Sprite(new Vector2(0, 0), playerSprite.Depth - .000001d, playerShieldTexture, new Rectangle(0, 0, 150, 150), Vector2.Zero);


            Rectangle shootinitialFrame = new Rectangle(0, 0, 33, 33);
            Texture2D shooterTexture = Content.Load<Texture2D>("Shooter");
            playerShooter = new Sprite(new Vector2(0, 0), 1d, shooterTexture, shootinitialFrame, Vector2.Zero);
            playerShooter.setDepth = false;

            
            // Load GUI resources
            hpMeterTexture = Content.Load<Texture2D>("UI/HealthMeter");
            shieldMeterTexture = Content.Load<Texture2D>("UI/ShieldMeter");
            energyMeterTexture = Content.Load<Texture2D>("UI/EnergyMeter");
            meterBackTexture = Content.Load<Texture2D>("UI/MeterBack");
            bottomBarTexture = Content.Load<Texture2D>("UI/BottomBar");
            
            // Mouse Resources
            mouseTexture = Content.Load<Texture2D>("UI/innermouse");
            shooterAimTexture = Content.Load<Texture2D>("UI/outermouse");
            mouseSprite = new Sprite(new Vector2(0, 0), 1d, mouseTexture, new Rectangle(0, 0, 64, 64), Vector2.Zero);
            mouseSprite.setDepth = false;

            Vector2 mousePosition = new Vector2(mosX, mosY);
            cursor.Initialize(mouseSprite, mousePosition);
            shooterAimSprite = new Sprite(new Vector2(0, 0), 1d, shooterAimTexture, new Rectangle(0, 0, 64, 64), Vector2.Zero);
            shooterAimSprite.setDepth = false;                                    
            enemyTexture = Content.Load<Texture2D>("Enemies/Enemy1");


            // projectile sprite stuff
            projectileTexture = Content.Load<Texture2D>("playerprojectile3");

            fireShotTexture = Content.Load<Texture2D>("ShotFireAnimation 2");

            eprojectileTexture = Content.Load<Texture2D>("slimeprojectile");

            // drops sprite/textures
            experienceDropTexture = Content.Load<Texture2D>("playerprojectile3");
            healthDropTexture = Content.Load<Texture2D>("playerprojectile3");

            // make explosion textures
            explosionTexture = Content.Load<Texture2D>("Enemies/Enemy1Explosion");

            // Load Icons
            blankIcon = Content.Load<Texture2D>("ui/icons/Blank");
            straightIcon = Content.Load<Texture2D>("ui/icons/Straight");
            scatterIcon = Content.Load<Texture2D>("ui/icons/Scatter");
            chargeIcon = Content.Load<Texture2D>("ui/icons/Charge");
            waveIcon = Content.Load<Texture2D>("ui/icons/Wave");

            // Load the music
            gameplayMusic = Content.Load<Song>("sound/Collidescope");

            // Load the laser and explosion sound effect
            laserSound = Content.Load<SoundEffect>("sound/laserFire");
            chargeSound = Content.Load<SoundEffect>("sound/charge");
            explosionSound = Content.Load<SoundEffect>("sound/slimedie");

            // load the score font
            font = Content.Load<SpriteFont>("gameFont");

            // Star the music right away
            PlayMusic(gameplayMusic);

            

            mainBackground = Content.Load<Texture2D>("mainbackground");

            bgLayer1.Initialize(Content, "backgroundScroll", GraphicsDevice.Viewport.Height, -1);



            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Save the previous state of the keyboard and game pad so we can determine single key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            MouseState mState = Mouse.GetState();
            mosX = mState.X;
            mosY = mState.Y;


            // Update the cursor
            UpdateCursor(gameTime);
            
            if (gameState == GameState.Play)
            {
                if (mState.RightButton == ButtonState.Pressed)
                {
                    CAmosX = mosX;
                    CAmosY = mosY;
                    gameState = GameState.ChooseAction;
                }

                // Allows the game to Pause
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    gameState = GameState.Pause;
                }
                if (currentKeyboardState.IsKeyUp(Keys.Escape) && escapeDown == true)
                {
                    escapeDown = false;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Escape) && escapeDown == false)
                {
                    escapeDown = true;
                    gameState = GameState.Pause;
                }

                screenOverLayTransparency = 0.0f;

                bgLayer1.Update();

                gotHit = gameTime.TotalGameTime;



                // Update Camera
                UpdateCamera(gameTime);
                UpdateGUI(gameTime);


                // Update the collision
                UpdateCollision();

                // Update the player
                UpdatePlayer(gameTime);
                playerShooter.Update(gameTime);
                playerShield.Update(gameTime);
                shooterAimSprite.Update(gameTime);

                // Update Drops
                UpdateDrops(gameTime);

                // Update the enemies
                UpdateEnemies(gameTime);

                // Update hit sprites
                UpdateHitShots(gameTime);

                // Update fire Shot Sprites
                UpdateFireShots(gameTime);

                // Update the projectiles
                UpdateProjectiles(gameTime);

                // Update enemy projectiles
                UpdateEnemyProjectile(gameTime);

                // Update Hit Texts
                damage.UpdateHitTexts(gameTime);

                // Update the explosions
                UpdateExplosions(gameTime);

                //Update Walls
                wall1.Update(gameTime);
                basewall.Update(gameTime);


                //test planet
                planet.Update(gameTime);
                planet2.Update(gameTime);
                planet3.Update(gameTime);

                //Update StarField
                starField.Update(gameTime);


            }

            if (gameState == GameState.Pause)
            {
                if (currentKeyboardState.IsKeyUp(Keys.Escape) && escapeDown == true)
                {
                    escapeDown = false;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Escape) && escapeDown == false)
                {
                    escapeDown = true;
                    gameState = GameState.Play;
                    
                }
                if (currentKeyboardState.IsKeyDown(Keys.Space) && escapeDown == false)
                {
                    this.Exit();
                }
                screenOverLayTransparency = 0.5f;
            }
            
            if (gameState == GameState.ChooseAction)
            {
                screenOverLayTransparency = 0.5f;
                if (!ChooseActionActive)
                {
                    ChooseActions();
                    ChooseActionActive = true;
                }
                if (mState.RightButton != ButtonState.Pressed)
                {
                    gameState = GameState.Play;
                    for (int i = CAIcons.Count - 1; i >= 0; i--)
                    {
                        if (CAIcons[i].Selected == true && CAIcons[i].Slot == 1) { projectileType = ProjectileType.Straight; }
                        if (CAIcons[i].Selected == true && CAIcons[i].Slot == 2) { projectileType = ProjectileType.Scatter; }
                        if (CAIcons[i].Selected == true && CAIcons[i].Slot == 3) { projectileType = ProjectileType.Charge; }
                        if (CAIcons[i].Selected == true && CAIcons[i].Slot == 4) { projectileType = ProjectileType.Wave; }
                        CAIcons.RemoveAt(i);
                    }
                    ChooseActionActive = false;
                }

                UpdateCollision();
                UpdateChooseActions(gameTime);
            }

            base.Update(gameTime);
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Frame == 10)
                {
                    explosions[i].Expired = true;
                }
                if (explosions[i].Expired == true)
                {
                    explosions.RemoveAt(i);
                }
            }
        }

        private void PlayMusic(Song song)
        {
            // due to the way the mediaplayer plays music,
            // we have to catch the exception. music will play when the game is not tethered
            try
            {
                // Play the music
                MediaPlayer.Play(song);

                // Loop the currently playing song
                MediaPlayer.IsRepeating = true;

                // Volume
                MediaPlayer.Volume = Sound;

            }
            catch { }
        }

        private void AddExplosion(Vector2 position, float depth, float rotation)
        {

            // set Frame Count
            int frameCount = 11;

            // Set initial frame, height and width
            Rectangle initialFrame = new Rectangle(0, 0, 150, 100);

            // Create the animation object
            Sprite explosion = new Sprite(position, depth, explosionTexture, initialFrame, Vector2.Zero);
            explosion.Rotation = rotation;
            explosion.FrameTime = .025f;
            for (int x = 1; x < frameCount; x++)
            {
                explosion.AddFrame(new Rectangle(initialFrame.X + (explosion.FrameWidth * x), initialFrame.Y, explosion.FrameWidth, explosion.FrameHeight));
            }

            explosions.Add(explosion);
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            // Update the projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update(gameTime);
                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        private void UpdateDrops(GameTime gameTime)
        {
            // Update the projectiles
            for (int i = drops.Count - 1; i >= 0; i--)
            {
                drops[i].Update(gameTime, player.PlayerSprite.WorldCenter);
                if (drops[i].Active == false)
                {
                    drops.RemoveAt(i);
                }
            }
        }

        private void UpdateFireShots(GameTime gameTime)
        {
            // Update the projectiles
            for (int i = fireShotSprites.Count - 1; i >= 0; i--)
            {
                
                fireShotSprites[i].Update(gameTime);
                if (fireShotSprites[i].StartTime)
                {
                    fireShotSprites[i].StartCounter = gameTime.TotalGameTime;
                    fireShotSprites[i].StartTime = false;
                }
                if (gameTime.TotalGameTime - fireShotSprites[i].StartCounter > fireShotSprites[i].expiredTime)
                {
                    fireShotSprites[i].Active = false;
                }
                if (fireShotSprites[i].Active == false)
                {
                    fireShotSprites.RemoveAt(i);
                }
            }
        }

        private void UpdateHitShots(GameTime gameTime)
        {
            // Update the projectiles
            for (int i = hitSprites.Count - 1; i >= 0; i--)
            {
                hitSprites[i].Update(gameTime);
                if (hitSprites[i].StartTime)
                {
                    hitSprites[i].StartCounter = gameTime.TotalGameTime;
                    hitSprites[i].StartTime = false;
                }
                if (gameTime.TotalGameTime - hitSprites[i].StartCounter > hitSprites[i].expiredTime)
                {
                    hitSprites[i].Active = false;
                }
                if (hitSprites[i].Active == false)
                {
                    hitSprites.RemoveAt(i);
                }
            }
        }

        private void UpdateEnemyProjectile(GameTime gameTime)
        {
   
            // Update the projectiles
            for (int i = enemyProjectiles.Count - 1; i >= 0; i--)
            {
                enemyProjectiles[i].Update(gameTime);
                if (enemyProjectiles[i].Active == false)
                {
                    enemyProjectiles.RemoveAt(i);
                }
            }
        }

        private void UpdateCursor(GameTime gameTime)
        {

            mouseSprite.RotateTo(mouseSprite.WorldLocation, player.Position);

            cursor.Update(gameTime);
        }

        private void AddProjectile(Vector2 position, Vector2 target)
        {
            float pScale;
            int pDamage;
            if (projectileType == ProjectileType.Straight) { Angle = 50; pScale = 1f; pDamage = 5;
            Projectile projectile = new Projectile(position, playerSprite.Depth, projectileTexture, new Rectangle(0, 0, 19, 19), Vector2.Zero, random, target + new Vector2(cursor.MouseSprite.FrameWidth / 2 - projectileTexture.Width / 2, cursor.MouseSprite.FrameHeight / 2 - projectileTexture.Height / 2), playerSprite, pScale, pDamage, Angle, Projectile.ParticleType.isplayer, player.Accuracy);
            
            projectiles.Add(projectile);
            }
            if (projectileType == ProjectileType.Scatter) { Angle = 1; pScale = .75f; pDamage = 3;
            Projectile projectile = new Projectile(position, playerSprite.Depth, projectileTexture, new Rectangle(0, 0, 19, 19), Vector2.Zero, random, target + new Vector2(cursor.MouseSprite.FrameWidth / 2 - projectileTexture.Width / 2, cursor.MouseSprite.FrameHeight / 2 - projectileTexture.Height / 2), playerSprite, pScale, pDamage, Angle, Projectile.ParticleType.isplayer, player.Accuracy);
            
            projectiles.Add(projectile);
            }
            if (projectileType == ProjectileType.Charge) { Angle = 100; pScale = 1 + (player.ChargeCounter / 100) * 4f; pScale = MathHelper.Clamp(pScale, 1, 5) ; pDamage = (int)player.ChargeCounter;
            Projectile projectile = new Projectile(position, playerSprite.Depth, projectileTexture, new Rectangle(0, 0, 19, 19), Vector2.Zero, random, target + new Vector2(cursor.MouseSprite.FrameWidth / 2 - projectileTexture.Width / 2, cursor.MouseSprite.FrameHeight / 2 - projectileTexture.Height / 2), playerSprite, pScale, pDamage, Angle, Projectile.ParticleType.isplayer, player.Accuracy);
            
            projectiles.Add(projectile);
            }

        }

        private void AddExperience(Vector2 position, double depth, Player player, int amount)
        {
            
            Drops experience = new Drops(position, depth, experienceDropTexture, new Rectangle(0, 0, 19, 19), Vector2.Zero, 1, player, amount, Drops.DropType.Experience);
            drops.Add(experience);
        }

        private void AddFireShotSprite(Vector2 position, Texture2D texture, Vector2 directionTarget)
        {
            Random randomRotation = new Random();
            Random randomScale = new Random();
            Projectile fireShot;

            float pRotation = (float)randomRotation.Next(-300, 300) / 100;
            float pScale = (float)randomScale.Next(3, 5) / 5;
            float cScale =  1 + (float)(player.ChargeCounter / 100) * 4f;
            cScale = MathHelper.Clamp(cScale, 1, 5);

            switch (projectileType)
            {
                case ProjectileType.Straight:
                    fireShot = new Projectile(position, playerSprite.Depth, texture, new Rectangle(0, 0, 33, 33), Vector2.Zero, random, Vector2.Zero, playerSprite, pScale, 0, pRotation, Projectile.ParticleType.isfiring, 0);
                    fireShotSprites.Add(fireShot);
                break;
                case ProjectileType.Scatter:
                fireShot = new Projectile(position, playerSprite.Depth, texture, new Rectangle(0, 0, 33, 33), Vector2.Zero, random, Vector2.Zero, playerSprite, pScale, 0, pRotation, Projectile.ParticleType.isfiring, 0);
                    fireShotSprites.Add(fireShot);
                break;
                case ProjectileType.Charge:
                fireShot = new Projectile(position, playerSprite.Depth, texture, new Rectangle(0, 0, 19, 19), Vector2.Zero, random, Vector2.Zero, playerSprite, cScale, 0, 0, Projectile.ParticleType.isfiring, 0);
                    fireShotSprites.Add(fireShot);
                break;
            }
        }

        private void AddHitSprite(Vector2 position, Sprite tSprite, Texture2D texture, Vector2 directionTarget)
        {
            Random randomRotation = new Random();
            Random randomScale = new Random();
            Projectile hitSprite;

            float pRotation = (float)randomRotation.Next(-300, 300) / 100;
            float pScale = (float)randomScale.Next(6, 8) / 5;

            hitSprite = new Projectile(position, tSprite.Depth, texture, new Rectangle(0, 0, 33, 33), Vector2.Zero, random, Vector2.Zero, playerSprite, pScale, 0, pRotation, Projectile.ParticleType.ishit, 0);
            hitSprites.Add(hitSprite);

        }

        private void AddEnemyProjectile(Vector2 position, Sprite iSprite, Enemy enemy)
        {
            int EnemyAngle = 40;
            
            Projectile enemyprojectile = new Projectile(position, iSprite.Depth, eprojectileTexture, new Rectangle(0, 0, 10, 10), Vector2.Zero, random, playerSprite.WorldLocation + playerSprite.RelativeCenter, playerSprite, 1f, 5, EnemyAngle, Projectile.ParticleType.isenemy, enemy.Accuracy);
            
            enemyProjectiles.Add(enemyprojectile);
        }

        private void AddChooseActionIcons(Texture2D icon, Vector2 position, int slot, int type)
        {
            ChooseActionIcons caIcon = new ChooseActionIcons();
            caIcon.Initialize(icon, position, slot, type);
            CAIcons.Add(caIcon);
        }

        public void UpdateChooseActions(GameTime gameTime)
        {
            for (int i = CAIcons.Count - 1; i >= 0; i--)
            {
                if (projectileType == ProjectileType.Straight && CAIcons[i].Slot == 1) { CAIcons[i].Active = true; }
                if (projectileType == ProjectileType.Scatter && CAIcons[i].Slot == 2) { CAIcons[i].Active = true; }
                if (projectileType == ProjectileType.Charge && CAIcons[i].Slot == 3) { CAIcons[i].Active = true; }
                if (projectileType == ProjectileType.Wave && CAIcons[i].Slot == 4) { CAIcons[i].Active = true; }
                CAIcons[i].Update(gameTime);
            }
        }

        private void ChooseActions()
        {
            // Projectile Selection
            AddChooseActionIcons(straightIcon, new Vector2(mosX + 40, mosY - 20), 1, 1);
            AddChooseActionIcons(scatterIcon, new Vector2(mosX + 20, mosY + 20), 2, 1);
            AddChooseActionIcons(chargeIcon, new Vector2(mosX - 20, mosY + 40), 3, 1);
            AddChooseActionIcons(waveIcon, new Vector2(mosX - 60, mosY + 20), 4, 1);

            // Unit Selection
            AddChooseActionIcons(blankIcon, new Vector2(mosX + 20, mosY - 60), 5, 2);
            AddChooseActionIcons(blankIcon, new Vector2(mosX - 20, mosY - 80), 6, 2);
            AddChooseActionIcons(blankIcon, new Vector2(mosX - 60, mosY - 60), 7, 2);

            // Item Selection
            AddChooseActionIcons(blankIcon, new Vector2(mosX - 80, mosY - 20), 8, 3);

        }

        private void UpdateCollision()
        {
            // Use the Rectangle's built-in intersect function to determine if two objects are overlapping
            Rectangle rectangle1;

            // Collision between mouse and Choose Action Icons
            for (int i = 0; i < CAIcons.Count; i++)
            {
                rectangle1 = new Rectangle(mosX, mosY, 5, 5);
                if (rectangle1.Intersects(CAIcons[i].IconRectangle))
                {
                    CAIcons[i].Selected = true;
                }
                else { CAIcons[i].Selected = false; }
            }

            // Do the collision between the player and the enemies
            for (int i = 0; i < enemies.Count; i++)
            {

                // Determine if the two objects collided with each other
                if (playerSprite.IsBoxColliding(enemies[i].EnemySprite.BoundingBoxRect) && enemies[i].EnemySprite.Depth == playerSprite.Depth)
                {                    
                    player.Position += enemies[i].enemyMovement * enemies[i].enemyMoveSpeed;
                    enemies[i].enemyMoveSpeed = .1f;
                    //enemies[i].enemyMoveSpeed = 0;
                    if (player.Hit == false)
                    {
                        //Subtract the health from the player based on the enemy damage
                        player.Hit = true;
                        player.Blink = true;
                        player.Health -= enemies[i].Damage;
                    }

                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                        player.Active = false;
                }
                else { enemies[i].enemyMoveSpeed = 3f; }
            }

            // Projectile vs enemy collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    // Determine if the two objects collided with each other
                    if (projectiles[i].IsCircleColliding(enemies[j].Position + enemies[j].EnemySprite.RelativeCenter, 20) && projectiles[i].Depth <= enemies[j].EnemySprite.Depth + .01d && projectiles[i].Depth >= enemies[j].EnemySprite.Depth - .01d)
                    {
                        enemies[j].Position += projectiles[i].projectileMovement * projectiles[i].projectileMoveSpeed *.1f;
                        
                        damage.DamageEnemy(player, projectiles[i], enemies[j], font);
                        
                        if (projectileType != ProjectileType.Charge || projectiles[i].Damage <= enemies[j].Health)
                        {
                            AddHitSprite(projectiles[i].WorldLocation - new Vector2(fireShotTexture.Width / 2, fireShotTexture.Height / 2) + projectiles[i].RelativeCenter, enemies[j].EnemySprite, fireShotTexture, Vector2.Zero);
                            projectiles[i].Active = false; 
                        }
                    }
                }
            }


            // Enemy Projectile vs Player
            for (int i = 0; i < enemyProjectiles.Count; i++)
            {

                enemyProjectiles[i].CollisionRadius = 2;

                // Determine if the two objects collided with each other
                if (enemyProjectiles[i].IsCircleColliding(playerShield.WorldCenter * (float)playerShield.DepthOffset, 30) && player.Shields > 0 && enemyProjectiles[i].Depth <= playerSprite.Depth + .01d && enemyProjectiles[i].Depth >= playerSprite.Depth - .01d)
                {

                    damage.DamagePlayer(player, enemyProjectiles[i], font);
                    enemyProjectiles[i].Active = false;

                    // If the player health is less than zero we died
                    if (player.Health <= 0) player.Active = false;
   
                }
            }

            // Drops colliding with Player
            for (int i = 0; i < drops.Count; i++)
            {
                drops[i].CollisionRadius = 2;
                if (drops[i].IsCircleColliding(playerShield.WorldCenter * (float)playerShield.DepthOffset, 30) && drops[i].Depth <= playerSprite.Depth + .01d && drops[i].Depth >= playerSprite.Depth - .01d)
                {
                    if (drops[i].dropType == Drops.DropType.Experience)
                    {
                        player.Experience += drops[i].amountOrid;
                        drops[i].Active = false;
                    }
                    if (drops[i].dropType == Drops.DropType.Heal)
                    {
                        drops[i].Active = false;
                    }
                }
            }

        }

        private void AddEnemy()
        {
 
            // Set initial frame, height and width
            Rectangle initialFrame = new Rectangle(0, 0, 100, 100);
            double checkDepth = random.Next(10, 50) / 10;
       
            // Create the animation object
            Sprite enemySprite = new Sprite(new Vector2(0, 0), 50, enemyTexture, initialFrame, Vector2.Zero);
            enemySprite.BoundingYPadding = 25;

            
            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(random.Next(enemySprite.FrameWidth, Camera.WorldRectangle.Width - enemySprite.FrameWidth), random.Next(enemySprite.FrameHeight, Camera.WorldRectangle.Height - enemySprite.FrameHeight));

            // Create an enemy
            Enemy enemy = new Enemy();

            // Initialize the enemy
            enemy.Initialize(enemySprite, position, 400f, Enemy.ElementType.Neutral);

            // Add the enemy to the active enemies list
            enemies.Add(enemy);
           
                              
        }
 
        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn a new enemy every X seconds
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime && enemies.Count < enemyCount)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                // Add an Enemy
                AddEnemy();
                
            }

            // Update the Enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (gameTime.TotalGameTime - enemies[i].previousEShootTime > enemies[i].enemyShootTime && enemies[i].IsEngaged == true)
                {

                    AddHitSprite(enemies[i].EnemySprite.WorldCenter * (float)enemies[i].EnemySprite.DepthOffset, enemies[i].EnemySprite, fireShotTexture, new Vector2(0, 0));
                    AddEnemyProjectile(enemies[i].EnemySprite.WorldCenter * (float)enemies[i].EnemySprite.DepthOffset, enemies[i].EnemySprite, enemies[i]);
                    enemies[i].previousEShootTime = gameTime.TotalGameTime;

                    //laserSound.Play(Sound, 1f, 0f);

                }
                enemies[i].Update(gameTime, player.Position, playerSprite);

                if (enemies[i].Active == false)
                {
                    // if not active and health <= 0
                    if (enemies[i].Health <= 0)
                    {
                        // Add an explosion
                        AddExplosion(enemies[i].Position - new Vector2(25, 0), (float)enemies[i].EnemySprite.Depth, enemies[i].EnemySprite.Rotation);
                        // Add Experience Drop
                        AddExperience(enemies[i].Position - new Vector2(25, 0), (float)enemies[i].EnemySprite.Depth, player, enemies[i].ExperienceValue);
                        // play the explosion sound
                        explosionSound.Play(Sound, 1.0f, 0f);
                        // add to the player's score
                        score += enemies[i].Value;
                        
                        enemyCount--;

                        if (enemyCount == 0)
                        {
                            wave++;
                            enemyCount = wave * 3;
                        }

                    }
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {

            // State of Mouse Click
            MouseState mouseClick = Mouse.GetState();
            
            int frameCount = 3;
            Rectangle initialFrame = new Rectangle(0, 0, 100, 150);
            playerSprite.Animate = false;
            for (int x = 1; x < frameCount; x++)
            {
                playerSprite.AddFrame(new Rectangle(initialFrame.X + (playerSprite.FrameWidth * x), initialFrame.Y, playerSprite.FrameWidth, playerSprite.FrameHeight));
            }
            playerSprite.BoundingXPadding = 25;
            playerSprite.BoundingYPadding = 40;
            
            player.Update(gameTime);


            if (projectileType == ProjectileType.Straight) { player.fireSeconds = 0.05f; player.ChargeCost = 6; }
            if (projectileType == ProjectileType.Scatter) { player.fireSeconds = 0.01f; player.ChargeCost = 2; }
            if (projectileType == ProjectileType.Charge) { player.fireSeconds = 1f; player.ChargeCost = 20; }

            if (player.Hit == true && player.Blink == true)
            {
                playerAlpha = .5f;
                player.PlayerSprite.TintColor = Color.White * playerAlpha;
                previousgotHit = gameTime.TotalGameTime + new TimeSpan(0, 0, 1);
                player.Blink = false;
            }
            if (gotHit > previousgotHit)
            {
                playerAlpha = 1.0f;
                player.PlayerSprite.TintColor = Color.White * playerAlpha;
                player.Hit = false;
            }
            playerSprite.Frame = 0;

            playerShield.WorldLocation = player.Position - new Vector2(35, 10);
            playerShield.Depth = playerSprite.Depth - .0001d;
            playerShield.TintColor = Color.White * .25f * player.ShieldPercentage;


            //Shooter Movement
            Vector2 aimcenter = player.Position + new Vector2(15, 35);



            float yMovement = cursor.MouseSprite.WorldLocation.Y;
            float xMovement = cursor.MouseSprite.WorldLocation.X;

            Vector2 projectileTarget;
            Vector2 circleplayerposition = aimcenter + new Vector2(125 * (float)Math.Cos((float)Math.Atan2(yMovement - aimcenter.Y, xMovement - aimcenter.X)), 125 * (float)Math.Sin((float)Math.Atan2(yMovement - aimcenter.Y, xMovement - aimcenter.X)));

            float shooterDistance = Vector2.Distance(aimcenter, circleplayerposition);
            float cursorDistance = Vector2.Distance(aimcenter, mouseSprite.WorldLocation);

            shooterAimSprite.RotateTo(shooterAimSprite.WorldLocation + shooterAimSprite.RelativeCenter, playerShooter.WorldLocation + playerShooter.RelativeCenter);
            
            playerShooter.WorldLocation = circleplayerposition + shooterAimSprite.RelativeCenter - playerShooter.RelativeCenter - new Vector2(65 * (float)Math.Cos((float)Math.Atan2(yMovement - aimcenter.Y, xMovement - aimcenter.X)), 65 * (float)Math.Sin((float)Math.Atan2(yMovement - aimcenter.Y, xMovement - aimcenter.X)));
            playerShooter.RotateTo(playerShooter.WorldLocation + playerShooter.RelativeCenter, shooterAimSprite.WorldLocation + shooterAimSprite.RelativeCenter);

            Vector2 fireLocation = circleplayerposition + shooterAimSprite.RelativeCenter - playerShooter.RelativeCenter - new Vector2(35 * (float)Math.Cos((float)Math.Atan2(yMovement - aimcenter.Y, xMovement - aimcenter.X)), 35 * (float)Math.Sin((float)Math.Atan2(yMovement - aimcenter.Y, xMovement - aimcenter.X)));
            Vector2 shootLocation = circleplayerposition + shooterAimSprite.RelativeCenter - playerShooter.RelativeCenter - new Vector2(35 * (float)Math.Cos((float)Math.Atan2(yMovement - aimcenter.Y, xMovement - aimcenter.X)), 35 * (float)Math.Sin((float)Math.Atan2(yMovement - aimcenter.Y, xMovement - aimcenter.X)));

            projectileTarget = shooterAimSprite.WorldLocation;

            if (cursorDistance <= shooterDistance)
            {
                shooterAimSprite.WorldLocation = circleplayerposition;

            }
            else
            {

                shooterAimSprite.WorldLocation = cursor.Position;

            }

            // Use the Keyboard / Dpad
            if (player.Hit == false)
            {
                if (currentKeyboardState.IsKeyDown(Keys.A) && !currentKeyboardState.IsKeyDown(Keys.D) ||
                    currentGamePadState.DPad.Left == ButtonState.Pressed)
                {
                    playerMoveX -= 0.05f;
                    playerSprite.Frame = 1;
                }

                if (currentKeyboardState.IsKeyDown(Keys.D) ||
                    currentGamePadState.DPad.Right == ButtonState.Pressed)
                {
                    playerMoveX += 0.05f;
                    playerSprite.Frame = 2;
                }

                if (currentKeyboardState.IsKeyDown(Keys.W) && !currentKeyboardState.IsKeyDown(Keys.S) ||
                    currentGamePadState.DPad.Up == ButtonState.Pressed)
                {
                    playerMoveY -= 0.05f;
                }
                if (currentKeyboardState.IsKeyDown(Keys.S) && !currentKeyboardState.IsKeyDown(Keys.W) ||
                    currentGamePadState.DPad.Down == ButtonState.Pressed)
                {
                    playerMoveY += 0.05f;
                }

            }

            
            if (playerMoveX > 0 && !currentKeyboardState.IsKeyDown(Keys.D))
            { playerMoveX -= 0.025f; playerMoveX = MathHelper.Clamp(playerMoveX, 0, 1); }
            else if (playerMoveX < 0 && !currentKeyboardState.IsKeyDown(Keys.A))
            { playerMoveX += 0.025f; playerMoveX = MathHelper.Clamp(playerMoveX, -1, 0); }

            if (playerMoveY > 0 && !currentKeyboardState.IsKeyDown(Keys.S))
            { playerMoveY -= 0.025f; playerMoveY = MathHelper.Clamp(playerMoveY, 0, 1); }
            else if (playerMoveY < 0 && !currentKeyboardState.IsKeyDown(Keys.W))
            { playerMoveY += 0.025f; playerMoveY = MathHelper.Clamp(playerMoveY, -1, 0); }

            player.Position.X += playerMoveSpeed * playerMoveX;
            player.Position.Y += playerMoveSpeed * playerMoveY;
            
            playerMoveX = MathHelper.Clamp(playerMoveX, -1, 1);
            playerMoveY = MathHelper.Clamp(playerMoveY, -1, 1);

            // Make Sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, Camera.WorldRectangle.X, Camera.WorldRectangle.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, Camera.WorldRectangle.Y, Camera.WorldRectangle.Height);




            
            if (mouseClick.LeftButton == ButtonState.Pressed)
            {
                
                //ballShooter.Animate = true;
                //fireShot.Animate = true;
                
                               
                if (projectileType == ProjectileType.Straight || projectileType == ProjectileType.Scatter)
                {

                    // Fire only every interval we set as the fireTime
                    if (gameTime.TotalGameTime - player.previousFireTime > player.fireTime && player.Charge >= player.ChargeCost)
                    {
                        // Reset our current time
                        player.previousFireTime = gameTime.TotalGameTime;

                        // Add the projectile randomly positioned near player; more shots equals greater distance.
                        AddProjectile(shootLocation + playerShooter.RelativeCenter - new Vector2(projectileTexture.Width / 2, projectileTexture.Height / 2), projectileTarget);
                        AddFireShotSprite(fireLocation + playerShooter.RelativeCenter- new Vector2(fireShotTexture.Width / 2, fireShotTexture.Height / 2), fireShotTexture, Vector2.Zero);

                        player.Charge -= player.ChargeCost;

                        // player Laser Sound
                        //laserSound.Play(Sound, 1f, 0f);

                    }
                }
                if (projectileType == ProjectileType.Charge)
                {
                    player.ChargeRelease = true;
                    if (player.Charge > 0)
                    {
                        player.ChargeCounter += 1f;
                    }
                    
                    AddFireShotSprite(shootLocation + playerShooter.RelativeCenter - new Vector2(projectileTexture.Width / 2, projectileTexture.Height / 2), projectileTexture, projectileTarget);
                    chargeSound.Play(Sound, 1f, 0f);
                    player.Charge -= 2;

                }

            }
            else
            {
                playerShooter.Frame = 0;
                playerShooter.Animate = false;
            }


            if (projectileType == ProjectileType.Charge && player.ChargeRelease == true)
            {
                if (mouseClick.LeftButton == ButtonState.Released)
                {
                    AddProjectile(shootLocation + playerShooter.RelativeCenter - new Vector2(projectileTexture.Width / 2, projectileTexture.Height / 2), projectileTarget);
                    player.ChargeRelease = false;
                    player.ChargeCounter = 0f;
                    // player Laser Sound
                    laserSound.Play(Sound, 1f, 0f);

                }
            }
            
            


            // reset score if player health goes to zero
            if (player.Health <= 0)
            {
                player.Health = 100;
                score = 0;
            }

            // Level up - Basic leveling system for now
            if (player.Experience >= player.Level * 3)
            {
                player.Level++;
                player.Experience = 0;
                player.TotalHealth += 20;
                player.Health = player.TotalHealth;
                player.Shots = player.Level;
            }

        }

        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }

        private void UpdateCamera(GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Up)) { Camera.Depth += .1d; }
            if (currentKeyboardState.IsKeyDown(Keys.Down)) { Camera.Depth -= .1d; }

            //Jupiter Distance
            if (currentKeyboardState.IsKeyDown(Keys.J))
            {
                Camera.ChangeDepth(2d, 0d);

            }

            //Earth Distance
            if (currentKeyboardState.IsKeyDown(Keys.E))
            {
                Camera.ChangeDepth(2d, 3900000000d);

            }

            //Mercury distance
            if (currentKeyboardState.IsKeyDown(Keys.M))
            {
                Camera.ChangeDepth(2d, 4470000000d);
                
            }
            
            Camera.Position = player.Position + playerSprite.RelativeCenter - new Vector2(640, 360);
        }

        private void UpdateGUI(GameTime gameTime)
        {

            int totalBar = (int)(281 * player.HPPercentage);
            int totalShield = (int)(281 * player.ShieldPercentage);
            //int totalCharge = 1043 - (int)(340 * player.ChargePercentage);
            int totalCharge = (int)(281 * player.ChargePercentage);
            float xoffset = 165f;
            
            //Meter Sprites
            playerHPMeter = new Sprite(new Vector2(0, 0), 1f, hpMeterTexture, new Rectangle(0, 0, totalBar, 16), Vector2.Zero);
            playerShieldMeter = new Sprite(new Vector2(0, 0), 1f, shieldMeterTexture, new Rectangle(0, 0, totalShield, 16), Vector2.Zero);
            playerEnergyMeter = new Sprite(new Vector2(0, 0), 1f, energyMeterTexture, new Rectangle(0, 0,  totalCharge, 16), Vector2.Zero);

            playerHPMeter.setDepth = false;
            playerShieldMeter.setDepth = false;
            playerEnergyMeter.setDepth = false;
            
            //Meter Backs
            meterBackHealth = new Sprite(new Vector2(0, 0), 1f, meterBackTexture, new Rectangle(0, 0, 281, 16), Vector2.Zero);
            meterBackShield = new Sprite(new Vector2(0, 0), 1f, meterBackTexture, new Rectangle(0, 0, 281, 16), Vector2.Zero);
            meterBackEnergy = new Sprite(new Vector2(0, 0), 1f, meterBackTexture, new Rectangle(0, 0, 281, 16), Vector2.Zero);

            meterBackHealth.setDepth = false;
            meterBackShield.setDepth = false;
            meterBackEnergy.setDepth = false;

            //Bottom Bar
            bottomBar = new Sprite(new Vector2(0, 0), 1f, bottomBarTexture, new Rectangle(0, 0, 1280, 50), Vector2.Zero);
            bottomBar.setDepth = false;

            //Update World Location of GUI
            playerHPMeter.WorldLocation = Camera.Position + new Vector2(xoffset, graphics.PreferredBackBufferHeight - playerHPMeter.FrameHeight - 33);
            playerShieldMeter.WorldLocation = Camera.Position + new Vector2(xoffset, graphics.PreferredBackBufferHeight - playerHPMeter.FrameHeight - 17);
            //playerEnergyMeter.WorldLocation = Camera.Position + new Vector2(totalCharge, graphics.PreferredBackBufferHeight - playerEnergyMeter.FrameHeight);
            playerEnergyMeter.WorldLocation = Camera.Position + new Vector2(xoffset, graphics.PreferredBackBufferHeight - playerEnergyMeter.FrameHeight - 1);

            meterBackHealth.WorldLocation = Camera.Position + new Vector2(xoffset, graphics.PreferredBackBufferHeight - meterBackHealth.FrameHeight - 33);
            meterBackShield.WorldLocation = Camera.Position + new Vector2(xoffset, graphics.PreferredBackBufferHeight - meterBackHealth.FrameHeight - 17);
            meterBackEnergy.WorldLocation = Camera.Position + new Vector2(xoffset, graphics.PreferredBackBufferHeight - meterBackEnergy.FrameHeight - 1);

            bottomBar.WorldLocation = Camera.Position + new Vector2(0, graphics.PreferredBackBufferHeight - bottomBar.FrameHeight);



            playerHPMeter.Update(gameTime);
            playerShieldMeter.Update(gameTime);
            playerEnergyMeter.Update(gameTime);
            meterBackHealth.Update(gameTime);
            meterBackShield.Update(gameTime);
            meterBackEnergy.Update(gameTime);
            bottomBar.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue * 0.0f);

            #region Draw Background
            spriteBatch.Begin();

            //Draw Background
            //spriteBatch.Draw(mainBackground, Vector2.Zero, new Rectangle(0, 0, 1280, 720), Color.White * .75f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.00001f);

            // Draw the moving background
            //bgLayer1.Draw(spriteBatch);

            //Draw StarField
            starField.Draw(spriteBatch);
            
            spriteBatch.End();
            #endregion

            #region Draw Player, Enemies, Projectiles, ETC
            for (int d = 4; d > 0; d--)
            {
                // Start drawing
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);


                //Draw the explosions
                for (int i = 0; i < explosions.Count; i++)
                {
                    if (explosions[i].DepthSequence == d)
                    {
                        explosions[i].Draw(spriteBatch);
                    }
                }

                // Draw the Enemies
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].EnemySprite.DepthSequence == d)
                    {
                        enemies[i].Draw(spriteBatch);
                    }
                }

                // Draw the Projectiles
                foreach (Projectile sprite in projectiles)
                {
                    if (sprite.DepthSequence == d)
                    {
                        sprite.Draw(spriteBatch);
                    }
                }
                // Draw the enemy projectiles
                foreach (Projectile sprite in enemyProjectiles)
                {
                    if (sprite.DepthSequence == d)
                    {
                        sprite.Draw(spriteBatch);
                    }
                }

                // Draw the Drops
                foreach (Drops sprite in drops)
                {
                    if (sprite.DepthSequence == d)
                    {
                        sprite.Draw(spriteBatch);
                    }
                }

                // Draw the fireShot sprites
                foreach (Projectile sprite in fireShotSprites)
                {
                    if (sprite.DepthSequence == d)
                    {
                        sprite.Draw(spriteBatch);
                    }
                }
                // draw the Hit Sprites
                foreach (Projectile sprite in hitSprites)
                {
                    if (sprite.DepthSequence == d)
                    {
                        sprite.Draw(spriteBatch);
                    }
                }

                // draw walls
                //wall1.Draw(spriteBatch);

                foreach (Sprite wall in wall1.walls)
                {
                    if (wall.DepthSequence == d)
                    {
                        wall.Draw(spriteBatch);
                    }
                }

                if (basewall.DepthSequence == d)
                {
                    basewall.Draw(spriteBatch);
                }



                // Draw the Player
                if (player.PlayerSprite.DepthSequence == d)
                {
                    player.Draw(spriteBatch);
                }
                if (playerShield.DepthSequence == d)
                {
                    playerShield.Draw(spriteBatch);
                }
                if (playerShooter.DepthSequence == d)
                {
                    playerShooter.Draw(spriteBatch);
                }


                //draw planet
                if (planet.DepthSequence == d)
                {
                    planet.Draw(spriteBatch);
                }
                if (planet2.DepthSequence == d)
                {
                    planet2.Draw(spriteBatch);
                }
                if (planet3.DepthSequence == d)
                {
                    planet3.Draw(spriteBatch);
                }

                // Stop drawing
                spriteBatch.End();
            }
            #endregion

            #region Draw GUI
            spriteBatch.Begin();

            // Draw the Hit Texts
            for (int i = 0; i < damage.hitTexts.Count; i++)
            {
                damage.hitTexts[i].Draw(spriteBatch);
            }

            // Draw the score
            spriteBatch.DrawString(font, "Score: " + score, new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.9999998f);
            
            // Saved for various text 
            //spriteBatch.DrawString(font, "Experience" + " " + player.Experience, new
            //Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 150), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.9999998f);
            //spriteBatch.DrawString(font, "player Layer" + " " + playerSprite.Layer, new
            //Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 180), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.9999998f);
            //spriteBatch.DrawString(font, "planet layer" + " " + planet.Layer, new
            //Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 210), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.9999998f);
            //spriteBatch.DrawString(font, "planet 2 depthoffset" + " " + planet2.DepthOffset, new
            //Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 240), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.9999998f);
            //spriteBatch.DrawString(font, "planet 2 layer" + " " + planet2.Layer, new
            //Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 270), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.9999998f);
            //spriteBatch.DrawString(font, "planet 2 depth sequence" + " " + planet2.DepthSequence, new
            //Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 300), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.9999998f);
            


            // Draw Screen Overlay for Pause
            spriteBatch.Draw(screenOverlay, new Vector2(0, 0), null, Color.Black * screenOverLayTransparency, 0.0f, Vector2.Zero, 320f, SpriteEffects.None, 0.99999f);

            if (gameState == GameState.Pause)
            {
                spriteBatch.DrawString(font, "Paused", new
                    Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0f, new Vector2(50, 0), 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(font, "(Press Space to Exit)", new
                    Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 + 30), Color.White, 0f, new Vector2(140, 0), 1f, SpriteEffects.None, 1f);
            }

            // Draw Choose Action GUI
            if (gameState == GameState.ChooseAction)
            {
                for (int i = 0; i < CAIcons.Count; i++)
                {
                    CAIcons[i].Draw(spriteBatch);
                }
            }

            // Draw GUI
            bottomBar.Draw(spriteBatch);
            meterBackHealth.Draw(spriteBatch);
            meterBackShield.Draw(spriteBatch);
            meterBackEnergy.Draw(spriteBatch);
            playerHPMeter.Draw(spriteBatch);
            playerShieldMeter.Draw(spriteBatch);
            playerEnergyMeter.Draw(spriteBatch);

            // Draw the player Level and Experience
            spriteBatch.DrawString(font, "Lv." + player.Level, new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 20, graphics.PreferredBackBufferHeight - 52), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.9999998f);
            spriteBatch.DrawString(font, "Exp. " + player.Experience, new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X+ 20, graphics.PreferredBackBufferHeight - 20), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0.9999998f);

            // Draw text for health, shield, energy
            spriteBatch.DrawString(font, "Health", new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 111, graphics.PreferredBackBufferHeight - 52), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0.9999998f);
            spriteBatch.DrawString(font, player.Health + "/" + player.TotalHealth, new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 280, graphics.PreferredBackBufferHeight - 52), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0.9999998f);

            spriteBatch.DrawString(font, "Shield", new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 114, graphics.PreferredBackBufferHeight - 36), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0.9999998f);
            spriteBatch.DrawString(font, (int)player.Shields + "/" + player.TotalShields, new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 280, graphics.PreferredBackBufferHeight - 36), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0.9999998f);


            spriteBatch.DrawString(font, "Energy", new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 111, graphics.PreferredBackBufferHeight - 20), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0.9999998f);
            spriteBatch.DrawString(font, player.Charge + "/" + player.TotalCharge, new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 280, graphics.PreferredBackBufferHeight - 20), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0.9999998f);



            // Draw Cursor
            cursor.Draw(spriteBatch);
            shooterAimSprite.Draw(spriteBatch);

            spriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }
    }
}
