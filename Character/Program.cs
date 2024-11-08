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
    }
    public void Attack (Character Opponent)
    {
        int damage = random.Next(1, DamagePower + 1);
        damage += IncreaseDamage;
        IncreaseDamage = 0;
        Opponent.Damage(damage);
        Console.WriteLine($"{Name} ударяет {Opponent.Name} с силой урона {damage}.");
    }
    public void Damage(int damage)
    {
        int dodgeChance = random.Next(0, 100);
        if (dodgeChance < Dodge)
        {
            Console.WriteLine($"{Name} увернулся от удара");
            return;
        }
        damage -= Defence;
        if (damage < 0) damage = 0;
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
                else
                {
                    Console.WriteLine("Неверный ввод. Пожалуйста , выберите1,2 или 3.");
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
                Console.WriteLine("Неверная команда. Введите Stsrt  или Exit");
            }
            
        }

        }
    }