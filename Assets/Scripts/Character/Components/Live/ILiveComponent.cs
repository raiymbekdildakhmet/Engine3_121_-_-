public interface ILiveComponent
{
    // Живой ли персонаж
    bool IsAlive { get; }

    // Получить урон
    void GetDamage(int damage);
}