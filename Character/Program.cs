using System;
using System.Collections.Specialized;
class Character
{
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int DamagePower { get; set; }
    public int HealPower { get; set; }
    public int Defence { get; set; }
    public int Dodge { get; set; }
    public int IncreaseDamage { get; internal set; }
    public Weapon Weapon { get; set; }
    public Armor Armor { get; set; }

    private Random random = new Random();
    public Character(string name)
    {
        Name = name;
        MaxHealth = random.Next(10, 21);
        CurrentHealth = MaxHealth;
        DamagePower = random.Next(3, 6);
        HealPower = random.Next(1, 4);
        Defence = random.Next(0, 3);
        Dodge = random.Next(0, 100);

        switch (random.Next(1, 4))
        {
            case 1: Weapon = new Sword(); break;
            case 2: Weapon = new CriticalWeapon (); break;
            case 3: Weapon = new Axe(); break;
        }

        switch (random.Next(1, 4))
        {
            case 1: Armor = new LightArmor (); break;
            case 2: Armor = new HeavyArmor(); break;
            case 3: Armor = new MagicArmor(); break;
        }
    }
    public void Info ()
    {
        Console.WriteLine($"Имя: {Name}");
        Console.WriteLine($"Максимальное здоровье: {MaxHealth}");
        Console.WriteLine($"Текущее здоровье:{CurrentHealth}");
        Console.WriteLine($"Сила урона:{DamagePower}");
        Console.WriteLine($"Сила лечения:{HealPower}");
        Console.WriteLine($"Защита:{Defence}");
        Console.WriteLine($"Вероятность уклонения:{Dodge}%");
        //Console.WriteLine($"Вы получили меч {Sword}");
        //Console.WriteLine($"Вы получили критическое оружие{CriticalWeapon}");
        //Console.WriteLine($"Вы получили топор{Axe}");
    }
    public void Attack (Character Opponent)
    {
        int damage = Weapon.Attack();
        damage = random.Next(damage, DamagePower + damage+1);
        damage += IncreaseDamage;
        IncreaseDamage = 0;
        Opponent.Damage(damage);
        Console.WriteLine($"{Name} ударяет {Opponent.Name} с силой урона {damage}.");
    }
    public void Damage(int damage)
    {
        damage = Armor.ReductDamage (damage);
        int dodgeChance = random.Next(0, 100);
        if (dodgeChance < Dodge)
        {
            Console.WriteLine($"{Name} увернулся от удара");
            return;
        }
        damage -= Defence;
        if (damage < 0) 
            damage = 0;
        CurrentHealth -= damage;
        Console.WriteLine($"{Name} получает урон. Здоровье: {CurrentHealth}/{MaxHealth}");

    }
    public void Heal ()
    {
        int healAmount = random.Next(1, HealPower + 1);
        CurrentHealth +=healAmount;
        if (CurrentHealth >MaxHealth) 
            CurrentHealth = MaxHealth;
        Console.WriteLine($"{Name} лечит себя на {healAmount}здоровья. Текущее здоровье: {CurrentHealth}/{MaxHealth}");
    }
}
class Game
{
    public Character FirstPlayer { get; set; }
    public Character SecondPlayer { get; set; }

    public void CreatePlayers()
    {
        Console.WriteLine("Введите имя первого игрока:");
        string name1 = Console.ReadLine();
        FirstPlayer = new Character(name1);
        Console.WriteLine("Введите имя второго игрока:");
        string name2 = Console.ReadLine();
        SecondPlayer = new Character(name2);
    }
    public void StartGame()
    {
        Random random = new Random();
        Console.WriteLine("Начинаем бой!!!");
        FirstPlayer.Info();
        SecondPlayer.Info();

        Character currentPlayer = FirstPlayer;

        while (FirstPlayer.CurrentHealth > 0 && SecondPlayer.CurrentHealth > 0)
        {
            Character opponent = currentPlayer == FirstPlayer ? SecondPlayer : FirstPlayer;
            Console.WriteLine($"nХод игрока {currentPlayer.Name}.Выберите действия указв цифру :");
            Console.WriteLine("1 - Атака");
            Console.WriteLine("2 - Лечение");
            Console.WriteLine("3 - Фокусировка");
            //Console.WriteLine("4 - Меч");
            //Console.WriteLine("5 - Крит.Оружие");
            //Console.WriteLine("6 - Топор");

            string action = Console.ReadLine();
            if (action == "1")
            {
                currentPlayer.Attack(opponent);

            }
            else
            {
                if (action == "2")
                {
                    currentPlayer.Heal();
                }
                else if (action == "3")
                {
                    currentPlayer.IncreaseDamage += random.Next(1, 4);
                    Console.WriteLine($"{currentPlayer.Name} фокусируется и увеличивает силу урона на {currentPlayer.IncreaseDamage}.");
                }
                //else
                //{
                //    if (action == "4")
                //    {
                //        currentPlayer.Weapon += random.Next(5,10);
                //    }
                //}
                else
                {
                    Console.WriteLine("Неверный ввод. Пожалуйста , выберите 1,2 или 3.");
                    continue;
                }
                if (opponent.CurrentHealth <= 0)
                {
                    Console.WriteLine($"{opponent.Name} проиграл бой!!!");
                    break;
                }
                currentPlayer = opponent;
            }
        }
  
   }
    
}

class Program
    {
    static void Main(string[] args)
    {
        //Random rand = new Random();
        //Character firstPlayer = new Character();
        //firstPlayer.Weapon = Random.Next(0, 3);
        //switch


        //{
        //    0 => new Sword(),
        //1 => new Axe(),
        //- => new CriticalWeapon()
        //}
            Game game = new Game();

            while (true)
            {
            Console.WriteLine($"Введите команду (Start/Exit):");
            string command = Console.ReadLine();
            if (command.Equals("Start", StringComparison.OrdinalIgnoreCase))
            {
                game.CreatePlayers();
                game.StartGame();
            }
            else if (command.Equals("Exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            else
            {
                Console.WriteLine("Неверная команда. Введите Start  или Exit");
            }
            
        }

        }
    }

abstract class Weapon
{
    public string Title { get; set; }
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
    public abstract int Attack();
}

    class Sword : Weapon
    {
        public Sword()
        {
            Title = "Sword";
            MinDamage = 5;
            MaxDamage = 10;
        }
        public override int Attack()
        {
            int damage = new Random().Next(MinDamage, MaxDamage + 1);
            Console.WriteLine($"Sword attack deals {damage} damage");
            return damage;
        }
    }
class Axe : Weapon
{
    public Axe()
    {
        Title = "Axe";
        MinDamage = 7;
        MaxDamage = 12;
    }
    public override int Attack()
    {
        int damage = new Random().Next(MinDamage, MaxDamage + 1);
        Console.WriteLine($"Axe attack deals {damage} damage");
        return damage;
    }
}
class CriticalWeapon : Weapon 
{
    public CriticalWeapon()
    {
        Title = "Critical Weapon";
        MinDamage = 7;
        MaxDamage = 12;
    }
    public override int Attack()
    {
        int damage = new Random().Next(MinDamage, MaxDamage + 1);
        if (new Random().Next(0, 2) == 0)
        {
            damage *= 2;
            Console.WriteLine($"Critical hit! {Title} deals {damage} damage");
           

        }
        else
        {
            Console.WriteLine($" {Title} deals {damage} damage");
        }
        return damage;
    }
}
abstract class Armor
{
    public int Defence { get; set; }
    public abstract int ReductDamage(int incomingDamage);
}
class LightArmor : Armor
{
    public LightArmor()
    {
        Defence = 3;
    }
    public override int ReductDamage(int incomingDamage)
    {
        if (new Random().Next(0, 2) == 0)
        {
            Console.WriteLine("Light armor deflected tge attack");
            return Math.Max(incomingDamage - Defence, 1);
        }
        return incomingDamage - Defence;
    }
}
class HeavyArmor : Armor
{
    public HeavyArmor()
    {
        Defence = 5;
    }
    public override int ReductDamage(int incomingDamage)
    {
        if (new Random().Next(0, 100) > 30)
        {
            Console.WriteLine("Heavy  armor absorbs the attack completely");
            return incomingDamage - (Defence * 2);
           
        }
        return incomingDamage - Defence;
    }

}
