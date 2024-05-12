# Приводим код в соответствие c Single-responsibility Principle

### Пример 1

- Было:
~~~C#
public static PushButtonData Create(string assembly, Command command)
{
    try
    {
        var type = command.GetType();
        var assemblyName = type.Assembly.GetName().Name;

        // В данном месте происходит перегрузка ответственностью (кроме создание кнопки мы также собираем изображение для него)
        var largeImage = new BitmapImage(
            new Uri($"{PathPrefix}{assemblyName}{PathPostfix}{command.Picture}{ImageFormat}"));

        return new PushButtonData(command.Name, command.Name, assembly, type.FullName)
        {
            LargeImage = largeImage,
            Image = new TransformedBitmap(largeImage, new ScaleTransform(0.5, 0.5)),
            ToolTip = command.Description,
            LongDescription = command.Version
        };
    }
    catch (Exception e)
    {
        TaskDialog.Show("Error",
            $@"During button creation {command.Name} {command.Version} error happened.

{e.Message} | {e.StackTrace}");
        throw;
    }
}
~~~
- Стало:
~~~C#
public static PushButtonData Create(string assembly, Command command)
{
    try
    {
        var type = command.GetType();
        var assemblyName = type.Assembly.GetName().Name;

        // Выносим создание изображения в отдельную функцию, чтобы не зависеть от его реализации
        var pushButtonData = pushButtonDataWithImage(assembly, command.Picture);
        
        pushButtonData.ToolTip = command.Description;
        pushButtonData.LongDescription = command.Version;

        return pushButtonData;
    }
    catch (Exception e)
    {
        TaskDialog.Show("Error",
            $@"During button creation {command.Name} {command.Version} error happened.
{e.Message} | {e.StackTrace}");
        throw;
    }
}

private PushButtonData pushButtonDataWithImage(assemblyName, Picture picture)
{
    var largeImage = new BitmapImage(new Uri($"{PathPrefix}{assemblyName}{PathPostfix}{picture}{ImageFormat}"));
    var image = new TransformedBitmap(largeImage, new ScaleTransform(0.5, 0.5));

    return new PushButtonData(command.Name, command.Name, assembly, type.FullName)
    {
        LargeImage = largeImage;
        Image = image;
    };
}
~~~

### Пример 2

- Было:
~~~C#
 public class Player : Obstacle
{
    // ...
    
    // Константы для перемещения игрока задаются непосредственно в нем, что нарушает принцип единой ответственности
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }
}
~~~
- Стало:
~~~C#
public class Player : Obstacle
{
    // ...
}

// Вынесли константы для перемещения на уровень выше, чтобы они были общими для всех, кто умеет передвигаться
public enum Direction
{
    Right,
    Left,
    Up,
    Down
}
~~~

### Пример 3

- Было:
~~~C#
public class Player : Obstacle
{
    // ...

    // Отображаем информацию об игроке непосредственно в самом игроке. При изменении отображения нам придется менять код класса игрока. 
    public void GetPlayerInfo()
    {
        if (Alive)
            Console.WriteLine($"{Name} has {ScorePoints} score points!");
        else
            Console.WriteLine($"{Name} was eaten by the monster...");
    }

    // ...
}
~~~
- Стало:
~~~C#
public class Player : Obstacle
{
    // ...
}

// За отображения информации об игроке отвечает отдельный класс. Он же отвечает за отображение любой информации о других существах.
public class Display
{
    public static void Player(Player player)
    {
        if (player.Alive)
            Console.WriteLine($"{player.Name} has {player.ScorePoints} score points!");
        else
            Console.WriteLine($"{player.Name} was eaten by the monster...");
    }
}
~~~
### Пример 4

- Было:
~~~C#
public class Player : Obstacle
{
    // ...

    public void Move(Direction direction)
    {
        int oldPosition = PositionX;
        bool directionX = true;

        switch (direction)
        {
            case Direction.Right:
                {
                    PositionX = PositionX + 1 <= _field.Width ? PositionX + 1 : PositionX;
                    break;
                }

            case Direction.Left:
                {
                    PositionX = PositionX - 1 > 0 ? PositionX - 1 : PositionX;
                    break;
                }

            case Direction.Up:
                {
                    oldPosition = PositionY;
                    directionX = false;
                    PositionY = PositionY + 1 <= _field.Height ? PositionY + 1 : PositionY;
                    break;
                }

            case Direction.Down:
                {
                    oldPosition = PositionY;
                    directionX = false;
                    PositionY = PositionY - 1 > 0 ? PositionY - 1 : PositionY;
                    break;
                }
        }

        // Метод определенно перегружен ответственностью: здесь мы и пытаемся поднять бонус, и проверяем атакует ли нас монстр
        foreach (var obstacle in _field.Obstacles)
        {
            if (obstacle is Bonus bonus && TryPick(bonus))
            {
                return;
            }

            if (obstacle is Monster monster && monster.CanAtack(this))
            {
                Alive = false;
                return;
            }

            if (obstacle.DoesIntersect(this))
            {
                if (directionX)
                    PositionX = oldPosition;
                else
                    PositionY = oldPosition;
                return;
            }
        }
    }
}
~~~
- Стало:
~~~C#
public class Player : Obstacle
{
    // ...

    public void Move(Direction direction)
    {
        var dirX = _directionX(direction);
        
        var lastPosition = ModifyPosition(direction);

        ProcessMovement(dirX, lastPosition);
    }

    private bool _directionX(Direction direction)
    {
        return direction is Direction.Left || direction is Direction.Right;
    }

    public int ModifyPosition(Direction direction)
    {
        int oldPosition = PositionX;
        
        switch (direction)
        {
            case Direction.Right:
                {
                    PositionX = PositionX + 1 <= _field.Width ? PositionX + 1 : PositionX;
                    break;
                }

            case Direction.Left:
                {
                    PositionX = PositionX - 1 > 0 ? PositionX - 1 : PositionX;
                    break;
                }

            case Direction.Up:
                {
                    oldPosition = PositionY;
                    PositionY = PositionY + 1 <= _field.Height ? PositionY + 1 : PositionY;
                    break;
                }

            case Direction.Down:
                {
                    oldPosition = PositionY;
                    PositionY = PositionY - 1 > 0 ? PositionY - 1 : PositionY;
                    break;
                }
        }

        return oldPosition;
    }

    public void ProcessMovement(bool directionX, int oldPosition)
    {
        foreach (var obstacle in _field.Obstacles)
        {
            if (obstacle is Bonus bonus && TryPick(bonus))
            {
                return;
            }

            if (obstacle is Monster monster && monster.CanAtack(this))
            {
                Alive = false;
                return;
            }

            if (obstacle.DoesIntersect(this))
            {
                if (directionX)
                    PositionX = oldPosition;
                else
                    PositionY = oldPosition;
                return;
            }
        }
    }
}
~~~
### Пример 5

- Было:
~~~C#
public class Player : Obstacle
{
    // ...

    public bool TryPick(Bonus bonus)
    {
        Obstacle obstacle = bonus as Obstacle;

        // Вычисляется возможность подбора бонуса, которую нужно вынести для разделения ответственности
        if (Math.Abs(PositionX - obstacle.PositionX) <= bonus.CollectionArea &&
                Math.Abs(PositionY - obstacle.PositionY) <= bonus.CollectionArea)
        {
            bonus.RemoveBonus(this);
            return true;
        }
        return false;
    }
}
~~~
- Стало:
~~~C#
public class Player : Obstacle
{
    // ...

    public bool TryPick(Bonus bonus)
    {
        if (validRange(bonus))
        {
            bonus.RemoveBonus(this);
            return true;
        }
        
        return false;
    }

    // Вынесли валидацию расстояния до препятствия
    private bool validRange(Bonus bonus)
    {
        Obstacle obstacle = bonus as Obstacle;
        return  Math.Abs(PositionX - obstacle.PositionX) <= bonus.CollectionArea &&
                Math.Abs(PositionY - obstacle.PositionY) <= bonus.CollectionArea;
    }
}
~~~