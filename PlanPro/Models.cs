using System;
using System.Collections.Generic;

namespace ProductionPlanner
{
    /// <summary>
    /// Ресурс (станок, оборудование)
    /// </summary>
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime NextFreeTime { get; set; }

        public Resource()
        {
            NextFreeTime = DateTime.Today.AddHours(9);
        }

        public override string ToString() => $"{Name} ({Type})";
    }

    /// <summary>
    /// Операция технологического процесса
    /// </summary>
    public class Operation
    {
        public int Seq { get; set; }
        public string Name { get; set; }
        public string NeedResource { get; set; }
        public int Hours { get; set; }
    }

    /// <summary>
    /// Изделие (тип продукции)
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Operation> Operations { get; set; }

        public Product()
        {
            Operations = new List<Operation>();
        }

        public override string ToString() => Name;
    }

    /// <summary>
    /// Заказ от клиента
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public Product Product { get; set; }
        public int Qty { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? EarliestStart { get; set; }
        public double PenaltyPerDay { get; set; }

        public Order()
        {
            Qty = 1;
            PenaltyPerDay = 500;
            Deadline = DateTime.Today.AddDays(7);
        }

        public override string ToString() => $"{Customer} - {Product?.Name}";
    }

    /// <summary>
    /// Элемент расписания (результат работы планировщика)
    /// </summary>
    public class ScheduleItem
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Resource Resource { get; set; }
        public Order Order { get; set; }
        public Operation Operation { get; set; }

        /// <summary>
        /// Просрочка: если конец работы ПОСЛЕ дедлайна
        /// </summary>
        public bool IsLate
        {
            get
            {
                return End > Order.Deadline;
            }
        }

        /// <summary>
        /// Количество дней просрочки (округление вверх)
        /// </summary>
        public int DaysLate
        {
            get
            {
                if (!IsLate) return 0;
                return (int)Math.Ceiling((End - Order.Deadline).TotalDays);
            }
        }

        /// <summary>
        /// Штраф за операцию
        /// </summary>
        public double Penalty => DaysLate * Order.PenaltyPerDay;
    }
}