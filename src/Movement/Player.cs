using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OstBot_2_
{

    public class Player : SynchronizedSprite
    {
        //protected var Ding:Class;
        //protected var Crown:Class;
        //protected var CrownSilver:Class;
        //protected var Aura:Class;
        //protected var ModAura:Class;
        //protected var ding:Sound;
        //private var world:World;
        //public var isme:Boolean;
        //private var crown:BitmapData;
        //private var crown_silver:BitmapData;
        //private var aura:BitmapData;
        //private var modaura:BitmapData;
        //private var connection:Connection;
        //private var state:PlayState;
        //private var chat:Chat;
        //private var badge:LevelBadge;
        //private bool badgeVisible = false;
        public string name;
        //private uint textcolor;
        private int morx = 0;
        private int mory = 0;
        public int woots = 0;
        public int coins = 0;
        public int level = 1;
        public int bcoins = 0;
        public bool hasCrown = false;
        public bool hasCrownSilver = false;
        public bool isgod = false;
        public bool ismod = false;
        public int current = 0;
        public int current_bg = 0;
        public bool purple = false;
        public int overlapy = 0;
        public double gravityMultiplier = 1;
        public double jumpMultiplier = 1;
        //private Rectangle rect2;
        public double aura_color = 4.29497e+009;
        public int aura_offset = 0;
        //private double total = 0;
        private int pastx = 0;
        private int pasty = 0;
        private List<int> queue;
        /*private DateTime lastJump;
        private bool changed = false;
        private int leftDown = 0;
        private int rightDown = 0;
        private int upDown = 0;
        private int downDown = 0;
        private bool spaceDown = false;
        private bool spaceJustDown = false;*/
        public int horizontal = 0;
        public int vertical = 0;
        public int oh = 0;
        public int ov = 0;
        private Point lastPortal;
        //private int lastOverlap = 0;
        public SynchronizedObject that = new SynchronizedObject();
        //private double bBest = 0;
        private bool donex = false;
        private bool doney = false;
        private double animoffset = 0;
        private double modoffset = 0;
        //private Rectangle modRect;
        //private bool slowMotion = true;


        int cx;
        int cy;
        bool isgodmod;
        double reminderX;
        double currentSX;
        double reminderY;
        double currentSY;
        double osx;
        double osy;
        double ox;
        double oy;
        //int mod;
        //bool injump;
        //bool cchanged;
        //bool doJump;
        double tx;
        double ty;

        public int ID;
        public bool bla, isFriend;
        public bool hasWonLevel = false;
        public Player(int ID, string name, int frame, float xPos, float yPos, bool isGod, bool isMod, bool bla, int coins, bool purple, bool isFriend, int level)
        {
            this.ID = ID;
            this.name = name;
            this.frame = frame; this.coins = coins; this.level = level;
            this.isgod = isGod; this.ismod = isMod; this.bla = bla; this.purple = purple; this.isFriend = isFriend;
            that.x = xPos;
            that.y = yPos;
            this.queue = new List<int>(Config.physics_queue_length);
        }

        public int overlaps(BlObject param1)
        {
            List<int> _loc_8 = new List<int>();
            //int _loc_10 = 0;
            int _loc_11 = 0;
            if (param1.x / 16 < 0 || param1.y / 16 < 0 || param1.x / 16 >= Form1.worldSize.X || param1.y / 16 >= Form1.worldSize.Y)
            {
                //Console.WriteLine("returning 1, worldborder");
                return 1;
            }
            Player _loc_2 = this;

            if (_loc_2.isgod || _loc_2.ismod)
            {
                //Console.WriteLine("returning 0, isgod");
                return 0;
            }

            double _loc_3 = ((_loc_2.x) / 16);
            double _loc_4 = ((_loc_2.y) / 16);
            //double _loc_5 = (param1.x + param1.height) / 16;
            //Console.WriteLine(_loc_3 + " - "  +_loc_5);
            //double _loc_6 = (param1.y + param1.width) / 16;
            //bool _loc_7 = false;
            for (int xx = -2; xx < 1; xx++)
            {
                for (int yy = -2; yy < 1; yy++)
                {
                    if (_loc_3 + xx > 0 && _loc_3 + xx < Form1.worldSize.X && _loc_4 + yy > 0 && _loc_4 + yy <= Form1.worldSize.Y)
                    {
                        for (int xTest = 0; xTest < 16; xTest++)
                        {
                            for (int yTest = 0; yTest < 16; yTest++)
                            {
                                if (hitTest((int)(xTest + _loc_2.x + xx * 16), (int)(yTest + _loc_2.y + yy * 16)))
                                {
                                    double _loc_9 = _loc_4;
                                    _loc_11 = Form1.blockMap[(int)(((xx * 16) + _loc_2.x + xTest) / 16), (int)(((yy * 16) + _loc_2.y + yTest) / 16)].ID;
                                    if (ItemId.isSolid(_loc_11))
                                    {
                                        switch (_loc_11)
                                        {
                                            case 23:
                                                {
                                                    //if (this.hideRed)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case 24:
                                                {
                                                    //if (this.hideGreen)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case 25:
                                                {
                                                    //if (this.hideBlue)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case 26:
                                                {
                                                    //if (!this.hideRed)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case 27:
                                                {
                                                    //if (!this.hideGreen)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case 28:
                                                {
                                                    //if (!this.hideBlue)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case 156:
                                                {
                                                    //if (this._hideTimedoor)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case 157:
                                                {
                                                    //if (!this._hideTimedoor)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case ItemId.DOOR_PURPLE:
                                                {
                                                    //if (this.hidePurple)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case ItemId.GATE_PURPLE:
                                                {
                                                    //if (!this.hidePurple)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case ItemId.COINDOOR:
                                                {
                                                    //if (this.getCoinValue(currentXBlock, currentRealPosY) <= coins)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case ItemId.COINGATE:
                                                {
                                                    //if (this.getCoinValue(currentXBlock, currentRealPosY) > this.showCoinGate)
                                                    {

                                                    }
                                                    break;
                                                }
                                            case 50:
                                                {
                                                    //if (isme)
                                                    {
                                                        // knownSecrets[currentXBlock + "x" + currentRealPosY] = true;
                                                    }
                                                    break;
                                                }
                                            case 61:
                                            case 62:
                                            case 63:
                                            case 64:
                                            case 89:
                                            case 90:
                                            case 91:
                                            case 96:
                                            case 97:
                                            case 122:
                                            case 123:
                                            case 124:
                                            case 125:
                                            case 126:
                                            case 127:
                                            case 146:
                                            case 154:
                                            case 158:
                                            case 194:
                                                {
                                                    if (_loc_2.speedY < 0 || _loc_9 <= _loc_2.overlapy)
                                                    {
                                                        if (_loc_9 != _loc_4 || _loc_2.overlapy == -1)
                                                        {
                                                            _loc_2.overlapy = (int)_loc_9;
                                                        }
                                                        //_loc_7 = true;
                                                        break;
                                                    }
                                                    break;
                                                }
                                            case 83:
                                            case 77:
                                                {
                                                    break;
                                                }
                                            default:
                                                {
                                                    break;
                                                }
                                        }
                                        return _loc_11;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //_loc_11 = Form1.blockMap[_loc_3, _loc_4].ID;
            /*while (_loc_9 < _loc_6)
            {

                _loc_8.Clear();
                for (int i = 0; i < Form1.worldSize.X; i++)
                {
                    _loc_8.Add(Form1.blockMap[i, _loc_9].ID);
                }
                _loc_10 = _loc_3;
                while (_loc_10 < _loc_5)
                {
                    if (_loc_8.Count != 0)
                    {
                        _loc_11 = _loc_8[_loc_10];
                        if (ItemId.isSolid(_loc_11))
                        {
                            switch(_loc_11)
                            {
                                case 61:
                                case 62:
                                case 63:
                                case 64:
                                case 89:
                                case 90:
                                case 91:
                                case 96:
                                case 97:
                                case 122:
                                case 123:
                                case 124:
                                case 125:
                                case 126:
                                case 127:
                                case 146:
                                case 154:
                                case 158:
                                case 194:
                                {
                                    if (_loc_2.speedY < 0 || _loc_9 <= _loc_2.overlapy)
                                    {
                                        if (_loc_9 != _loc_4 || _loc_2.overlapy == -1)
                                        {
                                            _loc_2.overlapy = _loc_9;
                                        }
                                        _loc_7 = true;
                                        break;
                                    }
                                    break;
                                }
                                case 83:
                                case 77:
                                {
                                    break;
                                }
                                default:
                                {
                                    break;
                                }
                            }
                            if (_loc_11 != 9)
                            Console.WriteLine("returning " + _loc_11 + ", collision");
                            return _loc_11;
                        }
                    }
                    _loc_10++;
                }
                _loc_9++;
            }
            if (!_loc_7)
            {
                _loc_2.overlapy = -1;
            }*/
            return 0;
        }// end function

        void stepx()
        {
            if (currentSX > 0)
            {
                if (currentSX + reminderX >= 1)
                {
                    x = x + (1 - reminderX);
                    x = Math.Floor(x);
                    currentSX = currentSX - (1 - reminderX);
                    reminderX = 0;
                }
                else
                {
                    x = x + currentSX;
                    currentSX = 0;
                }
            }
            else if (currentSX < 0)
            {
                if (reminderX != 0 && reminderX + currentSX < 0)
                {
                    currentSX = currentSX + reminderX;
                    x = x - reminderX;
                    x = Math.Floor(x);
                    reminderX = 1;
                }
                else
                {
                    x = x + currentSX;
                    currentSX = 0;
                }
            }
            if (overlaps(that) != 0)
            {
                //Console.WriteLine("xoverlap " + name);
                x = ox;
                _speedX = 0;
                currentSX = osx;
                donex = true;
            }
            return;
        }// end function

        void stepy()
        {
            if (currentSY > 0)
            {
                if (currentSY + reminderY >= 1)
                {
                    y = y + (1 - reminderY);
                    y = Math.Floor(y);
                    currentSY = currentSY - (1 - reminderY);
                    reminderY = 0;
                }
                else
                {
                    y = y + currentSY;
                    currentSY = 0;
                }
            }
            else if (currentSY < 0)
            {
                if (reminderY != 0 && reminderY + currentSY < 0)
                {
                    y = y - reminderY;
                    y = Math.Floor(y);
                    currentSY = currentSY + reminderY;
                    reminderY = 1;
                }
                else
                {
                    y = y + currentSY;
                    currentSY = 0;
                }
            }
            if (overlaps(that) != 0)
            {
                //Console.WriteLine("yoverlap " + name);
                y = oy;
                _speedY = 0;
                currentSY = osy;
                doney = true;
            }
            return;
        }// end function

        void processPortals()
        {
            List<Point> targetPortalList = new List<Point>();
            int loopIterator = 0;
            Point currentLoopPortal = new Point(0, 0);
            int _loc_4 = 0;
            int _loc_5 = 0;
            double _loc_6 = 0;
            double _loc_7 = 0;
            double _loc_8 = 0;
            double _loc_9 = 0;
            int _loc_10 = 0;
            double _loc_11 = 0;
            if (!isgodmod && current == 242)
            {
                if (lastPortal.X == 0 && lastPortal.Y == 0)
                {
                    lastPortal = new Point(cx << 4, cy << 4);

                    //current = Form1.blockMap[cx, cy].ID;
                    Block currentBlock = Form1.blockMap[cx, cy];
                    int currentTarget = currentBlock.targetID;
                    //Console.WriteLine("entered portal with id " + currentBlock.thisID + " and target id " + currentTarget + " and rotation " + currentBlock.rotation);

                    //targetPortalList = world.getPortals(world.getPortal(cx, cy).target);
                    for (int x = 1; x < Form1.worldSize.X; x++)
                    {
                        for (int y = 1; y < Form1.worldSize.Y; y++)
                        {
                            Block block = Form1.blockMap[x, y];
                            if (block.isPortal && block.thisID == currentTarget)
                            {
                                //Console.WriteLine("found portal target " + block.targetID);
                                targetPortalList.Add(new Point(x << 4, y << 4));
                            }
                        }
                    }
                    loopIterator = 0;
                    while (loopIterator < targetPortalList.Count)
                    {
                        //Console.WriteLine("iter: " + loopIterator);
                        currentLoopPortal = targetPortalList[loopIterator];
                        //_loc_4 = world.getPortal(lastPortal.x >> 4, lastPortal.y >> 4).rotation;
                        _loc_4 = Form1.blockMap[lastPortal.X >> 4, lastPortal.Y >> 4].rotation;
                        //Console.WriteLine("1: " + _loc_4);
                        //_loc_5 = world.getPortal(currentLoopPortal.x >> 4, currentLoopPortal.y >> 4).rotation;
                        _loc_5 = Form1.blockMap[currentLoopPortal.X >> 4, currentLoopPortal.Y >> 4].rotation;
                        //Console.WriteLine("2: " + _loc_5);
                        if (_loc_4 < _loc_5)
                        {
                            _loc_4 = _loc_4 + 4;
                        }
                        _loc_6 = speedX;
                        _loc_7 = speedY;
                        _loc_8 = modifierX;
                        _loc_9 = modifierY;
                        _loc_10 = _loc_4 - _loc_5;
                        _loc_11 = 1.42;
                        //Console.WriteLine("entering switch " + _loc_10);
                        switch (_loc_10)
                        {
                            case 1:
                                {
                                    speedX = _loc_7 * _loc_11;
                                    speedY = (-_loc_6) * _loc_11;
                                    modifierX = _loc_9 * _loc_11;
                                    modifierY = (-_loc_8) * _loc_11;
                                    reminderY = -reminderY;
                                    currentSY = -currentSY;
                                    break;
                                }
                            case 3:
                                {
                                    speedX = (-_loc_7) * _loc_11;
                                    speedY = _loc_6 * _loc_11;
                                    modifierX = (-_loc_9) * _loc_11;
                                    modifierY = _loc_8 * _loc_11;
                                    reminderX = -reminderX;
                                    currentSX = -currentSX;
                                    break;
                                }
                            case 2:
                                {
                                    speedX = (-_loc_6) * _loc_11;
                                    speedY = (-_loc_7) * _loc_11;
                                    modifierX = (-_loc_8) * _loc_11;
                                    modifierY = (-_loc_9) * _loc_11;
                                    reminderY = -reminderY;
                                    currentSY = -currentSY;
                                    reminderX = -reminderX;
                                    currentSX = -currentSX;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        /*if (isme && state)
                        {
                            state.offset(x - currentLoopPortal.x, y - currentLoopPortal.y);
                        }*/
                        //Console.WriteLine(currentLoopPortal + "  --------  " + x + " " + y);
                        x = currentLoopPortal.X;
                        y = currentLoopPortal.Y;
                        lastPortal = currentLoopPortal;
                        loopIterator++;
                        break;
                    }
                }
            }
            else
            {
                lastPortal = new Point(0, 0);
            }
            return;
        }// end function

        public void tick()
        {
            /*cx = 0;
            cy = 0;
            isgodmod = false;
            reminderX = 0;
            currentSX = 0;
            reminderY = 0;
            currentSY = 0;
            osx = 0;
            osy = 0;
            ox = 0;
            oy = 0;
            tx = 0;
            ty = 0;*/

            this.animoffset = this.animoffset + 0.2;
            if (this.ismod && !this.isgod)
            {
                this.modoffset = this.modoffset + 0.2;
                if (this.modoffset >= 16)
                {
                    this.modoffset = 10;
                }
            }
            else
            {
                this.modoffset = 0;
            }
            cx = (int)((x + 8) / 16);
            cy = (int)((y + 8) / 16);

            int delayed = 0;
            if (this.queue.Count >= 1)
            {
                delayed = this.queue[0];
                queue.Remove(delayed);
            }
            if (cx > 0 && cy > 0 && cx < Form1.worldSize.X && cy < Form1.worldSize.Y)
            {
                this.current = Form1.blockMap[cx, cy].ID;
            }
            queue.Add(this.current);
            if (this.current == 4 || ItemId.isClimbable(this.current))
            {
                if (this.queue.Count >= 1)
                {
                    delayed = this.queue[0];
                    queue.Remove(delayed);
                }
                this.queue.Add(this.current);
            }

            isgodmod = this.isgod || this.ismod;
            if (isgodmod)
            {
                this.morx = 0;
                this.mory = 0;
                this.mox = 0;
                this.moy = 0;
            }
            else
            {
                switch (this.current)
                {
                    case 1:
                        {
                            this.morx = -(int)_gravity;
                            this.mory = 0;
                            break;
                        }
                    case 2:
                        {
                            this.morx = 0;
                            this.mory = -(int)_gravity;
                            break;
                        }
                    case 3:
                        {
                            this.morx = (int)_gravity;
                            this.mory = 0;
                            break;
                        }
                    case ItemId.SPEED_LEFT:
                    case ItemId.SPEED_RIGHT:
                    case ItemId.SPEED_UP:
                    case ItemId.SPEED_DOWN:
                    case ItemId.CHAIN:
                    case ItemId.NINJA_LADDER:
                    case ItemId.WINE_H:
                    case ItemId.WINE_V:
                    case 4:
                        {
                            this.morx = 0;
                            this.mory = 0;
                            break;
                        }
                    case ItemId.WATER:
                        {
                            this.morx = 0;
                            this.mory = (int)_water_buoyancy;
                            Console.WriteLine("water");
                            break;
                        }
                    default:
                        {
                            this.morx = 0;
                            this.mory = (int)_gravity;
                            break;
                        }
                }
                switch (delayed)
                {
                    case 1:
                        {
                            this.mox = -_gravity;
                            this.moy = 0;
                            break;
                        }
                    case 2:
                        {
                            this.mox = 0;
                            this.moy = -_gravity;
                            break;
                        }
                    case 3:
                        {
                            this.mox = _gravity;
                            this.moy = 0;
                            break;
                        }
                    case ItemId.SPEED_LEFT:
                    case ItemId.SPEED_RIGHT:
                    case ItemId.SPEED_UP:
                    case ItemId.SPEED_DOWN:
                    case ItemId.CHAIN:
                    case ItemId.NINJA_LADDER:
                    case ItemId.WINE_H:
                    case ItemId.WINE_V:
                    case 4:
                        {
                            this.mox = 0;
                            this.moy = 0;
                            break;
                        }
                    case ItemId.WATER:
                        {
                            this.mox = 0;
                            this.moy = _water_buoyancy;
                            break;
                        }
                    default:
                        {
                            this.mox = 0;
                            this.moy = _gravity;
                            break;
                        }
                }
            }
            if (this.moy == _water_buoyancy)
            {
                mx = this.horizontal;
                my = this.vertical;
            }
            else if (this.moy != 0)
            {
                mx = this.horizontal;
                my = 0;
            }
            else if (this.mox != 0)
            {
                mx = 0;
                my = this.vertical;
            }
            else
            {
                mx = this.horizontal;
                my = this.vertical;
            }
            mox = mox * this.gravityMultiplier;
            moy = moy * this.gravityMultiplier;
            this.modifierX = this.mox + mx;
            this.modifierY = this.moy + my;
            if (_speedX != 0 || _modifierX != 0)
            {
                _speedX = _speedX + _modifierX;
                _speedX = _speedX * Config.physics_base_drag;
                if (mx == 0 && moy != 0 || _speedX < 0 && mx > 0 || _speedX > 0 && mx < 0 || ItemId.isClimbable(this.current) && !isgodmod)
                {
                    _speedX = _speedX * _no_modifier_dragX;
                }
                else if (this.current == ItemId.WATER && !isgodmod)
                {
                    _speedX = _speedX * _water_drag;
                }
                if (_speedX > 16)
                {
                    _speedX = 16;
                }
                else if (_speedX < -16)
                {
                    _speedX = -16;
                }
                else if (_speedX < 0.0001 && _speedX > -0.0001)
                {
                    _speedX = 0;
                }
            }
            if (_speedY != 0 || _modifierY != 0)
            {
                _speedY = _speedY + _modifierY;
                _speedY = _speedY * Config.physics_base_drag;
                if (my == 0 && mox != 0 || _speedY < 0 && my > 0 || _speedY > 0 && my < 0 || ItemId.isClimbable(this.current) && !isgodmod)
                {
                    _speedY = _speedY * _no_modifier_dragY;
                }
                else if (this.current == ItemId.WATER && !isgodmod)
                {
                    _speedY = _speedY * _water_drag;
                }
                if (_speedY > 16)
                {
                    _speedY = 16;
                }
                else if (_speedY < -16)
                {
                    _speedY = -16;
                }
                else if (_speedY < 0.0001 && _speedY > -0.0001)
                {
                    _speedY = 0;
                }
            }

            if (!isgodmod)
            {
                switch (this.current)
                {
                    case ItemId.SPEED_LEFT:
                        {
                            _speedX = -_boost;
                            break;
                        }
                    case ItemId.SPEED_RIGHT:
                        {
                            _speedX = _boost;
                            break;
                        }
                    case ItemId.SPEED_UP:
                        {
                            _speedY = -_boost;
                            break;
                        }
                    case ItemId.SPEED_DOWN:
                        {
                            _speedY = _boost;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                    /*case 100:
                    {
                        if (Global.play_sounds)
                        {
                            this.ding.play();
                        }
                        this.world.setTileComplex(0, cx, cy, 110, null);
                        var _loc_2:String = this;
                        var _loc_3:* = this.coins + 1;
                        _loc_2.coins = _loc_3;
                        cchanged;
                        break;
                    }
                    case 101:
                    {
                        if (Global.play_sounds)
                        {
                            this.ding.play();
                        }
                        this.world.setTileComplex(0, cx, cy, 111, null);
                        var _loc_2:String = this;
                        var _loc_3:* = this.bcoins + 1;
                        _loc_2.bcoins = _loc_3;
                        break;
                    }*/
                }
            }
            reminderX = x % 1;
            currentSX = _speedX;
            reminderY = y % 1;
            currentSY = _speedY;
            this.donex = false;
            this.doney = false;

            while (currentSX != 0 && !this.donex || currentSY != 0 && !this.doney)
            {
                this.processPortals();
                ox = x;
                oy = y;
                osx = currentSX;
                osy = currentSY;
                this.stepx();
                this.stepy();
            }

            if (this.pastx != cx || this.pasty != cy)
            {
                Random r = new Random();
                int i = r.Next(10);
                switch (this.current)
                {
                    case 5:
                        {
                            if (!this.hasCrown && !isgodmod)
                            {
                                //this.connection.send(Bl.data.m + "k");
                                Form1.connection.Send("say", this.name + " hit a crown!" + i);
                                //Console.WriteLine(name + " crown");
                            }
                            break;
                        }
                    case 6:
                        {
                            //this.connection.send(Bl.data.m + "r");
                            //this.state.showRed();
                            //Console.WriteLine("red");
                            Form1.connection.Send("say", this.name + " hit a red key!" + i);
                            //Console.WriteLine(name + " red");
                            break;
                        }
                    case 7:
                        {
                            //this.connection.send(Bl.data.m + "g");
                            //this.state.showGreen();
                            Form1.connection.Send("say", this.name + " hit a green key!" + i);
                            //Console.WriteLine(name + " green");
                            break;
                        }
                    case 8:
                        {
                            //this.connection.send(Bl.data.m + "b");
                            //this.state.showBlue();
                            Form1.connection.Send("say", this.name + " hit a blue key!" + i);
                            //Console.WriteLine(name + " blue");
                            break;
                        }
                    case ItemId.SWITCH_PURPLE:
                        {
                            this.purple = !this.purple;
                            if (!this.purple)
                            {
                                //this.state.hidePurple();
                            }
                            else
                            {
                                //this.state.showPurple();
                            }
                            //this.connection.send(Bl.data.m + "sp", this.purple);
                            break;
                        }
                    /*case 77:
                    {
                        if (this.pastx != cx || this.pasty != cy)
                        {
                            if (Global.play_sounds)
                            {
                                ItemManager.pianoSounds[this.world.getSound(cx, cy)].play();
                                this.world.pingSound(cx, cy);
                            }
                        }
                        break;
                    }
                    case 83:
                    {
                        if (this.pastx != cx || this.pasty != cy)
                        {
                            if (Global.play_sounds)
                            {
                                ItemManager.drumSounds[this.world.getSound(cx, cy)].play();
                                this.world.pingSound(cx, cy);
                            }
                        }
                        break;
                    }*/
                    case ItemId.DIAMOND:
                        {
                            //this.connection.send("diamondtouch", cx, cy);
                            break;
                        }
                    case ItemId.CAKE:
                        {
                            //this.connection.send("caketouch", cx, cy);
                            break;
                        }
                    case ItemId.BRICK_COMPLETE:
                        {
                            if (!isgodmod)
                            {
                                //this.connection.send("levelcomplete");
                                //if (!this.hascrownsilver)
                                {
                                    //Global.base.showLevelComplete(new LevelComplete());
                                }
                            }
                            break;
                        }
                }
                this.pastx = cx;
                this.pasty = cy;
            }

            double imx = _speedX * 256;
            double imy = _speedY * 256;
            moving = false;
            if (imx != 0 || this.current == ItemId.WATER)
            {
                moving = true;
            }
            else if (_modifierX < 0.1 && _modifierX > -0.1)
            {
                tx = x % 16;
                if (tx < 2)
                {
                    if (tx < 0.2)
                    {
                        x = Math.Floor(x);
                    }
                    else
                    {
                        x = x - tx / 15;
                    }
                }
                else if (tx > 14)
                {
                    if (tx > 15.8)
                    {
                        x = Math.Floor(x);
                        double _loc_3 = x + 1;
                        x = _loc_3;
                    }
                    else
                    {
                        x = x + (tx - 14) / 15;
                    }
                }
            }
            if (imy != 0 || this.current == ItemId.WATER)
            {
                moving = true;
            }
            else if (_modifierY < 0.1 && _modifierY > -0.1)
            {
                ty = y % 16;
                if (ty < 2)
                {
                    if (ty < 0.2)
                    {
                        y = Math.Floor(y);
                    }
                    else
                    {
                        y = y - ty / 15;
                    }
                }
                else if (ty > 14)
                {
                    if (ty > 15.8)
                    {
                        y = Math.Floor(y);
                        double _loc_3 = y + 1;
                        y = _loc_3;
                    }
                    else
                    {
                        y = y + (ty - 14) / 15;
                    }
                }
            }
            return;
        }// end function

        public void update()
        {
            return;
        }// end function

        public void resetCoins()
        {
            this.coins = 0;
            this.bcoins = 0;
            return;
        }// end function

    }
}
