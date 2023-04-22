using Event;

namespace GameInformation
{
	public sealed class Wallet
	{
		public int money { get; private set; }

		public CustomEventT<int>  onMoneyChanged { get; } = new();

		public Wallet(int pStartMoney)
		{
			money = pStartMoney;
			onMoneyChanged.Raise(money);
		}
	
		public void AddMoney(int pAmount)
		{
			money += pAmount;
			onMoneyChanged.Raise(money);
		}
	
		public bool RemoveMoney(int pAmount)
		{
			if (money - pAmount < 0) return false;
		
			money -= pAmount;
			onMoneyChanged.Raise(money);
			return true;
		}
		
		public bool CanAfford(int pAmount)
		{
			return money - pAmount >= 0;
		}
	}
}