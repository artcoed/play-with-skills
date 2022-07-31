interface IWeapon
{
    bool CanFire(IPlayer player);
    void Fire(IPlayer player);
}

class Weapon : IWeapon
{
    private readonly int _damage;

    private int _bullets;

    private bool HasBullets => _bullets > 0;

    public Weapon(int damage, int bullets)
    {
        if (_damage < 0)
            throw new ArgumentOutOfRangeException();

        if (bullets < 0)
            throw new ArgumentOutOfRangeException();

        _damage = damage;
        _bullets = bullets;
    }

    public bool CanFire(IPlayer player)
    {
        return player.CanTakeDamage(_damage) && HasBullets;
    }

    public void Fire(IPlayer player)
    {
        if (CanFire(player) == false)
            throw new ArgumentException();

        player.TakeDamage(_damage);
        _bullets -= 1;
    }
}

interface IPlayer
{
    bool IsDead { get; }
    
    bool CanTakeDamage(int amount);
    void TakeDamage(int amount);
}

class Player : IPlayer
{
    private IHealth _health;

    public bool IsDead => _health.IsEmpty;

    public Player(IHealth health)
    {
        _health = health;
    }

    public bool CanTakeDamage(int amount)
    {
        return _health.CanSpend(amount);
    }

    public void TakeDamage(int amount)
    {
        if (CanTakeDamage(amount) == false)
            throw new ArgumentOutOfRangeException();

        _health.Spend(amount);
    }
}

interface IHealth
{
    bool IsEmpty { get; }

    bool CanSpend(int amount);
    void Spend(int amount);
}

class Health : IHealth
{
    private int _value;

    public bool IsEmpty => _value == 0;

    public Health(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException();

        _value = value;
    }

    public bool CanSpend(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        return _value >= amount;
    }

    public void Spend(int amount)
    {
        if (CanSpend(amount) == false)
            throw new ArgumentOutOfRangeException();

        _value -= amount;
    }
}

interface IBot
{
    bool CanAttack(IPlayer player);
    void Attack(IPlayer player);
}

class Bot : IBot
{
    private Weapon _weapon;

    public Bot(Weapon weapon)
    {
        _weapon = weapon;
    }

    public void OnSeePlayer(Player player)
    {
        if (CanAttack(player))
            Attack(player);
    }

    public bool CanAttack(IPlayer player)
    {
        return _weapon.CanFire(player);
    }

    public void Attack(IPlayer player)
    {
        if (CanAttack(player) == false)
            throw new InvalidOperationException();

        _weapon.Fire(player);
    }
}
