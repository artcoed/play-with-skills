interface IWeapon
{
    bool CanFire(IHero hero);
    void Fire(IHero hero);
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

    public bool CanFire(IHero hero)
    {
        return hero.CanTakeDamage(_damage) && HasBullets;
    }

    public void Fire(IHero hero)
    {
        if (CanFire(hero) == false)
            throw new ArgumentException();

        hero.TakeDamage(_damage);
        _bullets -= 1;
    }
}

interface IHero
{
    bool IsDead { get; }
    
    bool CanTakeDamage(int amount);
    void TakeDamage(int amount);
}

class Hero : IHero
{
    private IHealth _health;

    public bool IsDead => _health.IsEmpty;

    public Hero(IHealth health)
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
    bool CanAttack(IHero hero);
    void Attack(IHero hero);
}

class Bot : IBot
{
    private Weapon _weapon;

    public Bot(Weapon weapon)
    {
        _weapon = weapon;
    }

    public bool CanAttack(IHero hero)
    {
        return _weapon.CanFire(hero);
    }

    public void Attack(IHero hero)
    {
        if (CanAttack(hero) == false)
            throw new InvalidOperationException();

        _weapon.Fire(hero);
    }
}
