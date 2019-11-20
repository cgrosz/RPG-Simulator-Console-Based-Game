using System;
using System.Collections.Generic;

/*
    TODO IDEA LIST
     - Random Maps that always have a path to victory
     - Make potions stackable in players inventory ?
 */
 /*
    BUG/FIX LIST
     - Shield not tanking damage on healer (maybe probably others)
     - /equip crashes when empty inventory
     - weapon damage boost is adding multiple times? maybe
     - crashes on win, just displays "You beat the level!" then prints map again

  */


namespace rpg
{
    
    class Human
    {
        public string Name;
        public int Strength;
        public int Intelligence;
        public int Dexterity;
        protected int health;
        public int spCount;
        public int killCount;

        public int exp;
        public List<string> moveSet;
        public string Guild;
        public List<Item> Inventory; //max size 28
        public Dictionary<string, int> Skills;

        public bool isWeaponEquiped;
        public Item currentEquipedWeapon;

        public bool isConsumableActive;
        public int consumableTicksLeft;
        public Item currentActiveConsumable;
         
        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }
         
        public Human(string name)
        {
            Guild = "default";
            Name = name;
            spCount = 3;
            Strength = 3;
            Intelligence = 3;
            Dexterity = 3;
            health = 100;
            Inventory = new List<Item>();
            Skills = new Dictionary<string,int>();
            initSkills();
            moveSet = new List<string>();
            killCount = 0;
            exp = 0;
            isWeaponEquiped = false;
            currentEquipedWeapon = null;
            isConsumableActive = false;
            consumableTicksLeft = 0;
            currentActiveConsumable = null;
        }
         
        public Human(string name, int str, int intel, int dex, int hp)
        {
            Guild = "default";
            Name = name;
            Strength = str;
            Intelligence = intel;
            Dexterity = dex;
            health = hp;
            moveSet = new List<string>();
        }
         
        // Build Attack method
        public virtual int Attack(Enemy target)
        {
            int dmg = Strength * 3;
            target.Health -= dmg;
            Console.WriteLine($"{Name} attacked {target.type} for {dmg} damage!");
            return target.Health;
        }

        //shield
        public virtual int Shield(Enemy enemy)
        {
            health = health + enemy.Level * 3;
            Console.WriteLine($"{Name} shielded themselves and blocked the attack!");
            return health;
        }
        //Healer heal
        public virtual int Heal(Human target){
            return 0;
        }
        //Attacker steal
        public virtual int Steal(Enemy target){
            return 0;
        }   
        //Defender meditate
        public virtual int Meditate(){
            return 0;
        }

        //healer ray special
        public virtual int GhostTrap(Enemy target)
        {
            return 0;
        }

        //attacker egon special
        public virtual int CrossTheStreams(Enemy target)
        {
            return 0;
        }

        //defender peter special
        public virtual int ManEatingToaster(Enemy target, Human me)
        {
            return 0;
        }
        
        //never used, cool idea tho
        public void initSkills() {
            Skills.Add("health", 100);
            Skills.Add("attack", 1);
            Skills.Add("strength", 1);
            Skills.Add("defence", 1);
            Skills.Add("speed", 1);
            Skills.Add("magic", 1);
            Skills.Add("ranged", 1);
            Skills.Add("herbalism", 1);
            Skills.Add("mining", 1);
            Skills.Add("slayer", 1);
        }

        public void Drop(int indexOfItemToDrop)
        {
            if(indexOfItemToDrop > 0 && indexOfItemToDrop <= Inventory.Count) {
                Inventory.Remove(Inventory[indexOfItemToDrop - 1]);
            }
        }

        public int Equip(Item wep)
        {   
            if(isWeaponEquiped) {
                Console.WriteLine($"You already have an item equiped!");
            }
            if(isWeaponEquiped == false) {
                Strength += wep.damageBoost;
                isWeaponEquiped = true;
                currentEquipedWeapon = wep;
            }   
            return Strength;
        }

        public int Unequip(Item wep)
        {
            if(isWeaponEquiped == false) {
                Console.WriteLine($"You don't have an item equiped!");
            }
            if(isWeaponEquiped == true) {
                Strength -= wep.damageBoost;
                isWeaponEquiped = false;
                currentEquipedWeapon = null;
            }
            return Strength;
        }

        public int Consume(Item pot, int indexOfConsumable)
        {
            consumableTicksLeft += 3;
            health += pot.amountHealed;
            Inventory.Remove(Inventory[indexOfConsumable]);
            currentActiveConsumable = pot;
            return health;
        }
        public int TickConsume() 
        {
            if(consumableTicksLeft > 0) 
            {
                health += currentActiveConsumable.amountHealed;
                consumableTicksLeft--;
            }
            return health;
        }
    }
    

    class Healer : Human
    {
        public Healer(string name) : base(name)
        {
            Guild = "Ray";
            //health = 50;
            health = 5000;
            Intelligence = 25;
            spCount = 3;
            moveSet.Add("Basic Attack");
            moveSet.Add("Heal");
            moveSet.Add("Shield");
            moveSet.Add("Ghost Trap");
        }
        public override int Attack(Enemy target)
        {
            int dmg = Strength * 5;
            target.Health -= dmg;
            //health += health + dmg;
            Console.WriteLine($"{Name} attacked {target.type} for {dmg} damage!");
            return target.Health;
        }
        public override int Heal(Human target) 
        {
            int healed_health = Intelligence * 10;
            target.Health += healed_health;
            Console.WriteLine($"{Name} healed {target.Name} for {healed_health} health!");
            return target.Health;
        }

        public override int GhostTrap(Enemy target)
        {
            if(spCount > 0) {
                int dmg = this.health * 2;
                target.Health -= dmg;
                Console.WriteLine($"{Name} Ghost Trapped {target.type} for {dmg} damage!");
                spCount--;
                return target.Health;
            }
            Console.WriteLine($"{Name} is out of special attacks!");
            return target.Health;
        }
    }

    class Attacker : Human
    {
        public Attacker(string name) : base(name)
        {
            Guild = "Egon";
            Dexterity = 175;
            spCount = 3;
            moveSet.Add("Basic Attack");
            moveSet.Add("Steal");
            moveSet.Add("Cross The Streams");
        }
        public override int Attack(Enemy target)
        {
            int dmg = Dexterity * 5;
            Random rng = new Random();
            int extra = rng.Next(1,6);
            if(extra == 6) {
                dmg += 10;
            }
            target.Health -= dmg;
            Console.WriteLine($"{Name} attacked {target.type} for {dmg} damage!");
            return target.Health;
        }
        public override int Steal(Enemy target) 
        {
            target.Health -= 5;
            Health += 5;
            return target.Health;
        }
        public override int CrossTheStreams(Enemy target)
        {
            if(spCount > 0) {
                Attack(target);
                Attack(target);
                Attack(target);
                spCount--;
                return target.Health;
            } else {
                Console.WriteLine($"{Name} hurt itself in confusion - out of special attacks");
                return target.Health;
            }
        }
    }

    class Defender : Human
    {
        public Defender(string name) : base(name)
        {
            Guild = "Peter";
            health = 200;
            spCount = 3;
            moveSet.Add("Basic Attack");
            moveSet.Add("Meditate");
            moveSet.Add("Shield");
            moveSet.Add("Man Eating Toaster");
        }
        public override int Attack(Enemy target)
        {
            base.Attack(target);
            if(target.Health < 50) {
                target.Health = 0;
            }
            return target.Health;
        }
        public override int Meditate()
        {
            health = 200;
            Console.WriteLine($"{Name} healed themselves to full health!");
            return health;
        }
        public override int ManEatingToaster(Enemy target, Human me)
        {
            if(spCount > 0) {
                Random rnd = new Random();
                int isDead = rnd.Next(0,2);
                if(isDead == 1) {
                    me.Health = 0;
                    Console.WriteLine($"{Name}'s got in the way on the Man Eating Toaster and died");
                    return me.Health;
                } else {
                    target.Health = 0;
                    Console.WriteLine($"{Name}'s Man Eating Toaster slayed {target.type}");
                    return target.Health;
                }
            }
            return target.Health;
        }
    }

    class Enemy
    {
        public int Level;
        public int Strength;
        protected int health;
        public string type;
         
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
         
        public Enemy(int level)
        {
            type = "Enemy";
            Level = level;
            health = 100;
            Strength = 5;
        }
         
        // Build Attack method
        public virtual int Attack(Human target)
        {
            int dmg = Strength * 3;
            target.Health -= dmg;
            Console.WriteLine($"{type} attacked {target.Name} for {dmg} damage!");
            return target.Health;
        }
        
    }

    class Nanny : Enemy
    {
        public Nanny(int level) : base(level)
        {
            type = "Ghost Nanny";
            health = 5*level;
            Strength = 2*level;
        }
    }

    class Brothers : Enemy
    {
        public Brothers(int level) : base(level)
        {
            type = "Scoleri Brothers";
            health = 25*level;
            Strength = 9*level;
        }
    }

    class Zuul : Enemy
    {
        public Zuul(int level) : base(level)
        {
            type = "Zuul";
            health = 15*level;
            Strength = 11*level;
        }
    }

    class Slimer : Enemy
    {
        public Slimer(int level) : base(level)
        {
            type = "Slimer";
            health = 20*level;
            Strength = 2*level;
        }
    }

    class Puft : Enemy
    {
        public Puft(int level) : base(level)
        {
            type = "Stay Puft Marshmellow Man";
            health = 100*level;
            Strength = 0*level;
        }
    }

    class Item
    {
        public string name;
        public bool stackable;
        public bool isPotion;
        public int amountHealed;
        public int damageBoost;
        public int usesLeft;
        
        // Potion Constructor, some weapons
        public Item(string itemName, int amount)
        {
            name = itemName;
            amountHealed = amount;
        }

        // wallbreaker/durability items constructor
        public Item(string itemName, int attackDamage, int uses)
        {

        }
    }

    class Consumable : Item
    {
        public Consumable(string potName, int amount) : base(potName, amount)
        {
            name = potName;
            stackable = true;
            isPotion = true;
            amountHealed = amount/4;     
        }
    }

    class Weapon : Item
    {
        public Weapon(string wepName, int amount) : base(wepName, amount)
        {
            name = wepName;
            stackable = false;
            damageBoost = amount;
        }
    }

    class DurabilityWeapon : Item
    {
        public DurabilityWeapon(string wepName, int damage, int uses) : base(wepName, damage, uses)
        {
            name = wepName;
            stackable = false;
            damageBoost = damage;
            usesLeft = uses;
        }
    }
        
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Human player = pickClass();
            Console.WriteLine($"Now, start your adventure... Good luck, {player.Name}");
            Console.WriteLine($"Use /help to open up the controls");
            //string[,] map = initMap();
            //string [,] map = generateRandomMap();
            int[] playerCoords = new int[2]{1,1};
            string [,] map = createMap(playerCoords);
            int[] lastLoc = new int[2];
            displayMap(map, playerCoords);
            int currentLevel = 0;

            while(true) {
                string keyPressed = Console.ReadLine();
                lastLoc[0] = playerCoords[0];
                lastLoc[1] = playerCoords[1];
                playerCoords = movePlayer(playerCoords, keyPressed, map, player);
                if(playerCoords[0] == lastLoc[0] && playerCoords[1] == lastLoc[1]) {
                    continue;
                }
                if(playerCoords[0] == 99) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You Beat The Level!");
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    //map = generateRandomMap();
                    playerCoords = new int[2]{1,1};
                    map = createMap(playerCoords);
                    lastLoc = new int[2];
                    displayMap(map, playerCoords);
                    currentLevel++;
                    continue;
                }

                map = updateMap(map, lastLoc, playerCoords);
                Console.WriteLine($"Level: {currentLevel}");
                displayMap(map, playerCoords);
                Random rnd = new Random();
                int chance = rnd.Next(0,3);

                int calculateEnemyLevel = -1;
                Random rndEnemy = new Random();
                while(calculateEnemyLevel < 0) {
                    calculateEnemyLevel = rndEnemy.Next(-3,4);
                }
                int enemyLevel = calculatePlayerLevel(player) + calculateEnemyLevel;
                Nanny a = new Nanny(enemyLevel);
                Brothers b = new Brothers(enemyLevel);
                Zuul c = new Zuul(enemyLevel);
                Slimer d = new Slimer(enemyLevel);
                Puft e = new Puft(enemyLevel);
                List<Enemy> randomEnemy = new List<Enemy>();
                randomEnemy.Add(a);
                randomEnemy.Add(b);
                randomEnemy.Add(c);
                randomEnemy.Add(d);
                randomEnemy.Add(e);
                
                if(chance == 2) {
                    Random spawnEnemy = new Random();
                    int spawnEnemyNumber = spawnEnemy.Next(0,5);
                    Encounter(player, randomEnemy[spawnEnemyNumber]);
                    displayMap(map, playerCoords);
                }
                if(player.Health <= 0)  {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You Perished the Haunted House...");
                    Console.ResetColor();
                    break;
                }
            }           
        }

        static string[,] generateRandomMap()
        {
            string [,] map = new string[12,36]
            {
                {"\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680"},
                {"\u2680",".",".",".","\u2680",".",".",".",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".",".",".","\u2680","\u2680","\u2680",".",".",".",".",".","\u2680"},
                {"\u2680",".",".",".","\u2680",".",".",".",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".","\u2680","\u2680","\u2680","\u2680","\u2680",".",".",".",".",".","\u2680"},                
                {"\u2680",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","\u2680","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".",".","\u2680"},                
                {"\u2680","\u2680",".","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680",".",".",".",".",".","\u2680","\u2680","\u2680","\u2680",".",".",".","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".",".","\u2680"},                
                {"\u2680",".",".",".",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".","\u2680","\u2680",".",".",".",".",".","\u2680",".",".",".",".",".",".","\u2680"},                
                {"\u2680",".",".",".",".",".",".",".",".",".","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680",".","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","\u2680"},
                {"\u2680",".",".",".",".",".",".",".","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".","\u2680","\u2680",".","\u2680"},
                {"\u2680",".",".","\u2680","\u2680",".",".",".",".",".",".",".",".",".",".","\u2680",".",".",".","\u2680","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".","\u2680",".","\u2680",".","\u2680"},                
                {"\u2680",".",".","\u2680","\u2680",".",".",".","\u2680","\u2680",".",".",".","\u2680",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".",".",".","\u2680",".","\u2680"},                
                {"\u2680",".",".",".",".",".",".",".","\u2680","\u2680",".",".",".",".",".","\u2680",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".",".",".",".","\u2680",".","\u2680"},
                {"\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","W","\u2680"},
           
            };
            for(int i = 1; i < 11; i++) {
                for(int j = 1; j < 35; j++) {
                    Random rnd = new Random();
                    int isWall = rnd.Next(0,10);
                    if(isWall == 3 || isWall == 4) {
                        map[i,j] = "\u2680";
                    } else {
                         map[i,j] = ".";
                    }
                    Random rnd2 = new Random();
                    int isItem = rnd2.Next(0,100);
                    if(isItem == 99) {
                        map[i,j] = "\u2931";
                    }
                }
            }
            // refactor map building
            // map each tile the player can walk on a Tile class instance
            // create a hasVisited bool in there
            // randomly assign wall / empty tile to the Tile when creating the map
            // this allows me to run Dijksta's algorithm over a randomly generated map
            // and ensure that there is always a path to the end point
            return map;
        }

        static string [,] createMap(int[] coords)
        {
            string [,] newMap = generateRandomMap();
            string [,] checkMap = addFlagsToMap(newMap);
            //displayMap(newMap, playerCoords);
            //string itemToConsume = Console.ReadLine();
            int check = hasPathToVictory(checkMap, coords);
            Console.WriteLine(check);
            while(check < 999999999) {
                //string itemToConsume = Console.ReadLine();
                checkMap = generateRandomMap();
                checkMap = addFlagsToMap(checkMap);
            }
            return newMap;
        }
       
        static int hasPathToVictory(string [,] map, int[] coords)
        {
            string[] hasVisited = map[coords[0],coords[1]].Split(':');
            map[coords[0],coords[1]] = hasVisited[0] + ":true";

            Console.WriteLine(map[coords[0],coords[1]]);
            //base case - found map
            if(map[coords[0],coords[1]] == "W:false" || map[coords[0],coords[1]] == "W:true" ){
                return 1000000000;
            }

            // recursively check each map tile
            if(map[coords[0]+1,coords[1]] == ".:false" || map[coords[0]+1,coords[1]] == "\u2931:false"){
                int[] playerCoords = new int[2]{coords[0]+1,coords[1]};
                displayMap(map, playerCoords);
                return hasPathToVictory(map, playerCoords);
            } 
            if(map[coords[0]-1,coords[1]] == ".:false" || map[coords[0]-1,coords[1]] == "\u2931:false"){
                int[] playerCoords = new int[2]{coords[0]-1,coords[1]};
                displayMap(map, playerCoords);
                return hasPathToVictory(map, playerCoords);
            }
            if(map[coords[0],coords[1]+1] == ".:false" || map[coords[0],coords[1]+1] == "\u2931:false"){
                int[] playerCoords = new int[2]{coords[0],coords[1]+1};
                displayMap(map, playerCoords);
                return hasPathToVictory(map, playerCoords);
            }
            if(map[coords[0],coords[1]-1] == ".:false" || map[coords[0],coords[1]-1] == "\u2931:false"){
                int[] playerCoords = new int[2]{coords[0],coords[1]-1};
                displayMap(map, playerCoords);
                return hasPathToVictory(map, playerCoords);
            }
            return 0;

        }
        
        static string[,] addFlagsToMap(string [,] map)
        {
            for(int i = 0; i < 12; i++) {
                for(int j = 0; j < 36; j++) {
                    string newString = map[i,j] + ":false";
                    map[i,j] = newString;
                }
            }
            return map;
        }

        static string[,] initMap()
        {
            string [,] map = new string[12,36]
            {
                {"\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680"},
                {"\u2680",".",".",".","\u2680",".",".",".",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".",".",".","\u2680","\u2680","\u2680",".",".",".",".",".","\u2680"},
                {"\u2680",".",".",".","\u2680",".",".",".",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".","\u2680","\u2680","\u2680","\u2680","\u2680",".",".",".",".",".","\u2680"},                
                {"\u2680",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","\u2680","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".",".","\u2680"},                
                {"\u2680","\u2680",".","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680",".",".",".",".",".","\u2680","\u2680","\u2680","\u2680",".",".",".","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".",".","\u2680"},                
                {"\u2680",".",".",".",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".","\u2680","\u2680",".",".",".",".",".","\u2680",".",".",".",".",".",".","\u2680"},                
                {"\u2680",".",".",".",".",".",".",".",".",".","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680",".","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","\u2680"},
                {"\u2680",".",".",".",".",".",".",".","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".","\u2680","\u2680",".","\u2680"},
                {"\u2680",".",".","\u2680","\u2680",".",".",".",".",".",".",".",".",".",".","\u2680",".",".",".","\u2680","\u2680","\u2680","\u2680",".",".",".",".",".",".",".",".","\u2680",".","\u2680",".","\u2680"},                
                {"\u2680",".",".","\u2680","\u2680",".",".",".","\u2680","\u2680",".",".",".","\u2680",".",".",".",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".",".",".","\u2680",".","\u2680"},                
                {"\u2680",".",".",".",".",".",".",".","\u2680","\u2680",".",".",".",".",".","\u2680",".",".",".","\u2680",".",".",".",".",".",".",".",".",".",".",".",".",".","\u2680",".","\u2680"},
                {"\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","\u2680","W","\u2680"},
           
            };
            return map;
        }

        static void displayMap(string[,] map, int[] player) 
        {
            for(int i = 0; i < 12; i++) {
                for(int j = 0; j < 36; j++) {
                    if(i == player[0] && j == player[1]) {
                        map[i,j] = "\u263A";
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(map[i,j]);
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    } else {
                        string[] tileToDisplay = map[i,j].Split(':');
                        Console.Write(tileToDisplay[0]);
                        //Console.Write(map[i,j]);
                    }   
                }
                Console.WriteLine("");
            }
        }

        static void CheckRandomEvent(string[,] map, int[] coords)
        {
            Random rnd = new Random();
            int chance = rnd.Next(0,6); 
            string[] events = {
                "You found a friendly Ghost, gain 1 special point",
                "You find a free sandwich!",
                "You accidently run into a miror and break it, bad luck for you",
                "You stumbled across a full bowl of Holloween candy left by Anne, say thanks to her",
                "Find some enriched uranium and shove it in your proton pack.",
                "You see a blue sequin tuxedo jacket draped over a chair. It's so tacky! You can't look directly at it....",
                "A spirit asks 'are you a god?' You say no like an idiot.",
                "You see Slimer eating and entire ham. He sees you coming and drops the ham as he floats away. You pick up the ham and take a bite for science.",
                "You slip on some slime and fall on your ass. How embarrassing.",
            };
            if(chance == 5) {
                Random rnd2 = new Random();
                int msgChance = rnd2.Next(0,9);
                Console.WriteLine($"{events[msgChance]}");
            }
        }

        static int[] movePlayer(int[] coords, string dir, string[,] map, Human player)
        {
            int[] didWin = {99,99};
            bool won = false;
            int currentEquipedItemIndex = 0;
            // PLAYER MOVEMENT
            if(dir == "w") {
                if(player.currentEquipedWeapon != null && player.currentEquipedWeapon.name == "pickaxe" && map[coords[0]-1,coords[1]] == "\u2680") {
                    player.TickConsume();
                    CheckRandomEvent(map, coords);
                    int[] arr = {coords[0]-1, coords[1]};
                    won = checkWin(map, arr);
                    coords[0]--;

                    player.currentEquipedWeapon.usesLeft--;
                    if(player.currentEquipedWeapon.usesLeft <= 0) {
                        Console.WriteLine($"Your {player.currentEquipedWeapon.name} has broke.");
                        player.currentEquipedWeapon.name = $"broken {player.currentEquipedWeapon.name}";
                        player.currentEquipedWeapon.damageBoost = 2;
                        player.Unequip(player.currentEquipedWeapon);
                    }

                    if(won) {
                        return didWin ;
                    }               
                }
                else if(map[coords[0]-1,coords[1]] != "\u2680"){ 
                    if(map[coords[0]-1,coords[1]] == "\u2931") {
                        Consumable potion = new Consumable("health pot", 100);
                        player.Inventory.Add(potion);
                        Console.WriteLine($"You scavenged a {potion.name}");
                    }
                    player.TickConsume();
                    CheckRandomEvent(map, coords);
                    int[] arr = {coords[0]-1, coords[1]};
                    won = checkWin(map, arr);
                    coords[0]--;
                    if(won) {
                        return didWin ;
                    }
                }
            } else if (dir == "a") {
                if(player.currentEquipedWeapon != null && player.currentEquipedWeapon.name == "pickaxe" && map[coords[0],coords[1]-1] == "\u2680") {
                    player.TickConsume();
                    CheckRandomEvent(map, coords);
                    int[] arr = {coords[0], coords[1]-1};
                    won = checkWin(map, arr);
                    coords[1]--;
                    
                    player.currentEquipedWeapon.usesLeft--;
                    if(player.currentEquipedWeapon.usesLeft <= 0) {
                        Console.WriteLine($"Your {player.currentEquipedWeapon.name} has broke.");
                        player.currentEquipedWeapon.name = $"broken {player.currentEquipedWeapon.name}";
                        player.currentEquipedWeapon.damageBoost = 2;
                        player.Unequip(player.currentEquipedWeapon);
                    }

                    if(won) {
                        return didWin ;
                    }               
                }
                else if(map[coords[0],coords[1]-1] != "\u2680"){
                    if(map[coords[0],coords[1]-1] == "\u2931") {
                        Consumable potion = new Consumable("health pot", 100);
                        player.Inventory.Add(potion);
                        Console.WriteLine($"You scavenged a {potion.name}");
                    }
                    player.TickConsume();
                    CheckRandomEvent(map, coords);
                    int[] arr = {coords[0], coords[1]-1};
                    won = checkWin(map, arr);
                    coords[1]--;
                    if(won) {
                        return didWin ;
                    }
                }
            } else if (dir == "s") {
                if(player.currentEquipedWeapon != null && player.currentEquipedWeapon.name == "pickaxe" && map[coords[0]+1,coords[1]] == "\u2680") {
                    player.TickConsume();
                    CheckRandomEvent(map, coords);
                    int[] arr = {coords[0]+1, coords[1]};
                    won = checkWin(map, arr);
                    coords[0]++;
                    
                    player.currentEquipedWeapon.usesLeft--;
                    if(player.currentEquipedWeapon.usesLeft <= 0) {
                        Console.WriteLine($"Your {player.currentEquipedWeapon.name} has broke.");
                        player.currentEquipedWeapon.name = $"broken {player.currentEquipedWeapon.name}";
                        player.currentEquipedWeapon.damageBoost = 2;
                        player.Unequip(player.currentEquipedWeapon);
                    }

                    if(won) {
                        return didWin ;
                    }               
                }
                else if(map[coords[0]+1,coords[1]] != "\u2680"){
                    if(map[coords[0]+1,coords[1]] == "\u2931") {
                        Consumable potion = new Consumable("health pot", 100);
                        player.Inventory.Add(potion);
                        Console.WriteLine($"You scavenged a {potion.name}");
                    }
                    player.TickConsume();
                    CheckRandomEvent(map, coords);
                    int[] arr = {coords[0]+1, coords[1]};
                    won = checkWin(map, arr);
                    coords[0]++;
                    if(won) {
                        return didWin ;
                    }
                }
            } else if (dir == "d") {
                if(player.currentEquipedWeapon != null && player.currentEquipedWeapon.name == "pickaxe" && map[coords[0],coords[1]+1] == "\u2680") {
                    player.TickConsume();
                    CheckRandomEvent(map, coords);
                    int[] arr = {coords[0], coords[1]+1};
                    won = checkWin(map, arr);
                    coords[1]++;

                    player.currentEquipedWeapon.usesLeft--;
                    if(player.currentEquipedWeapon.usesLeft <= 0) {
                        Console.WriteLine($"Your {player.currentEquipedWeapon.name} has broke.");
                        player.currentEquipedWeapon.name = $"broken {player.currentEquipedWeapon.name}";
                        player.currentEquipedWeapon.damageBoost = 2;
                        player.Unequip(player.currentEquipedWeapon);
                    }

                    if(won) {
                        return didWin ;
                    }              
                }
                else if(map[coords[0],coords[1]+1] != "\u2680"){
                    if(map[coords[0],coords[1]+1] == "\u2931") {
                        Consumable potion = new Consumable("health pot", 100);
                        player.Inventory.Add(potion);
                        Console.WriteLine($"You scavenged a {potion.name}");
                    }
                    player.TickConsume();
                    CheckRandomEvent(map, coords);
                    int[] arr = {coords[0], coords[1]+1};
                    won = checkWin(map, arr);
                    coords[1]++;
                    if(won) {
                        return didWin ;
                    }
                } 
            // COMMANDS
            } else if(dir == "help" || dir == "/help") {
                Console.WriteLine("Controls:");
                Console.WriteLine(" - Use WASD and number keys to play");
                Console.WriteLine(" - Use /inv to open your inventory");
                Console.WriteLine(" - Use /equip to equip weapons");
                Console.WriteLine(" - Use /unequip to unequip weapons");
                Console.WriteLine(" - Use /consume to use consumables");
                Console.WriteLine(" - Use /drop to drop items from ur inventory");
            } else if(dir == "/inv") {
                int counter = 1;
                foreach(Item item in player.Inventory) {
                    Console.Write($"{counter}. {item.name} || ");
                    counter++;
                }
                Console.WriteLine(" ");
            } else if(dir == "/equip") {
                Console.Write($"Which item do u want to equip? ");
                string itemToEquip = Console.ReadLine();
                int item = Int32.Parse(itemToEquip);
                currentEquipedItemIndex = item-1;
                player.Equip(player.Inventory[item-1]);
            } else if(dir == "/unequip") {
                if(player.isWeaponEquiped == true) {
                    player.Unequip(player.Inventory[currentEquipedItemIndex]);
                }
            } else if(dir == "/consume") {
                Console.Write($"Which item do u want to consume? ");
                string itemToConsume = Console.ReadLine();
                int item = Int32.Parse(itemToConsume);
                player.Consume(player.Inventory[item-1], item-1);
            } else if(dir == "/drop") {
                Console.Write($"Which item do u want to drop? ");
                string itemToConsume = Console.ReadLine();
                int item = Int32.Parse(itemToConsume);
                player.Drop(item);
            // testing purposes only, generates new map
            } else if(dir == "/gen") {
                coords[0] = 99;
            }
            return coords;
        }

        static bool checkWin(string[,] map, int[] coords) {
            if(map[coords[0],coords[1]] == "W") {
                return true;
            }
            return false;
        }

        static string[,] updateMap(string[,] map, int[] prevLoc, int[] coords) 
        {
            string [,] newMap = new string[12,36];
            for(int i = 0; i < 12; i++) {
                for(int j = 0; j < 36; j++) {
                    if(i == coords[0] && j == coords[1]) {
                        newMap[coords[0],coords[1]] = "o";
                        //Console.Write(newMap[i,j]);
                    } else if(i == prevLoc[0] && j == prevLoc[1]){
                        newMap[prevLoc[0],prevLoc[1]] = ".";
                        //Console.Write(newMap[prevLoc[0],prevLoc[1]]);
                    } else {
                        newMap[i,j] = map[i,j];
                        //newMap[i,j] = ".";
                        //Console.Write(newMap[i,j]);
                    }
                }
                //Console.WriteLine("");
            }
            return newMap;
        }

        static Human pickClass()
        {
            Console.WriteLine("Who's Calling?");
            string playerName = Console.ReadLine();
            Console.WriteLine("Who you gunna call?");
            Console.WriteLine($"Hi, {playerName}. Choose your Ghostbuster!\n1. Ray(Healer)\n2. Egon(Attacker)\n3. Peter(Defender)");
            string playerType = Console.ReadLine();

            if(playerType == "1") {
                Healer player = new Healer($"{playerName}");
                //Console.WriteLine($"{player.Name}");
                return player;
            } else if(playerType == "2") {
                Attacker player = new Attacker($"{playerName}");
                //Console.WriteLine($"{player.Name}");
                return player;
            } else if(playerType == "3") {
                Defender player = new Defender($"{playerName}");
                //Console.WriteLine($"{player.Name}");
                return player;
            } else {
                Console.WriteLine("Please input 1, 2, or 3.");
            }
            return null;
        }

        static Human Encounter(Human player, Enemy enemy)
        {
            Console.WriteLine($"{player.Name} has encountered a level {enemy.Level} {enemy.type}!");
            Console.WriteLine($"Press 'X' To Run Away");
            int playerLevel = calculatePlayerLevel(player);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Player Level: {playerLevel}    ||||| Monsters Slain: {player.killCount}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{player.Name}'s Health: {player.Health} ||||| {enemy.type}'s Health: {enemy.Health}");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            int grantedEXP = enemy.Health;
            while(player.Health > 0 && enemy.Health > 0) {
                Console.WriteLine($"What is your move?");
                for(int i = 1; i <= player.moveSet.Count; i++) {
                    Console.WriteLine($"{i}. {player.moveSet[i-1]}");
                }
                string playerAttack = Console.ReadLine();
                if(playerAttack == "1") {
                    player.Attack(enemy);
                } else if(playerAttack == "2") {
                    if(player.Guild == "Ray") {
                        player.Heal(player);
                    } else if(player.Guild == "Egon") {
                        player.Steal(enemy);
                    } else if(player.Guild == "Peter") {
                        player.Meditate();
                    }
                } else if(playerAttack == "3") {
                    if(player.Guild == "Ray") {
                        player.Shield(enemy);
                    } else if(player.Guild == "Peter") {
                        player.Shield(enemy);
                    } else if(player.Guild == "Egon") {
                        player.CrossTheStreams(enemy);
                    }
                } else if(playerAttack == "4") {
                    if(player.Guild == "Ray") {
                        player.GhostTrap(enemy);
                    } else if(player.Guild == "Peter") {
                        player.ManEatingToaster(enemy, player);
                    }
                } else if(playerAttack =="x"  ||  playerAttack =="X"){
                    break;
                }
                else {
                    continue;
                }
                if(enemy.Health <= 0) {
                    continue;
                }
                enemy.Attack(player);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{player.Name}'s Health: {player.Health} ||||| {enemy.type}'s Health: {enemy.Health}");
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.DarkGray;   
            }
            Console.ForegroundColor = ConsoleColor.Red;
            if(enemy.Health <= 0) {
                player.killCount++;
                player.exp = player.exp + grantedEXP;
                int newPlayerLevel = calculatePlayerLevel(player);
                Console.WriteLine("You killed the enemy!");

                Weapon testWep = new Weapon("testWep",100);
                DurabilityWeapon pickaxe = new DurabilityWeapon("pickaxe",20, 5);
                

                Random rnd = new Random();
                int dropChance = rnd.Next(0,5);
                if(dropChance == 2) {
                    if(player.Inventory.Count < 29) {
                        player.Inventory.Add(testWep);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"You found a {testWep.name}");
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.DarkGray;   
                    }
                }
                if(dropChance == 3) {
                    if(player.Inventory.Count < 29) {
                        player.Inventory.Add(pickaxe);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"You found a {pickaxe.name}");
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.DarkGray;   
                    }
                }

                if(playerLevel != newPlayerLevel) {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Congrats! You leveled up to {newPlayerLevel}");
                }
            } else if(player.Health <= 0) {
                Console.WriteLine("Oh dear, you are dead...");
            } else {
                Console.WriteLine("You successfully escaped!!");
            }
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.DarkGray; 
            return player;
        }

        static int calculatePlayerLevel(Human player) 
        {
            int playerLevel = 0;
            int maxLevel = 9;
            int [,] levels = new int[9,3]
            {
                {1, 0, 83},
                {2, 84, 210},
                {3, 211, 903},
                {4, 904, 3201},
                {5, 3202, 8982},
                {6, 8983, 20123},
                {7, 20124, 74823},
                {8, 74824, 213034},
                {9, 213034, 1000000}
            };

            for(int i = 0; i < maxLevel; i++) {
                if(player.exp >= levels[i,1] && player.exp <= levels[i,2]) {
                    playerLevel = levels[i,0];
                }
            }
            return playerLevel;
        }
    }
}
