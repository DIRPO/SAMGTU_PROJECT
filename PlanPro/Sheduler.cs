using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProductionPlanner
{
    public class Scheduler
    {
        private List<Resource> _resources;
        private List<Order> _orders;

        public Scheduler(List<Resource> resources, List<Order> orders)
        {
            _resources = resources;
            _orders = orders;
        }

        public List<ScheduleItem> Build()
        {
            List<ScheduleItem> result = new List<ScheduleItem>();
            int nextId = 1;

            // Сброс времени ресурсов
            foreach (Resource r in _resources)
            {
                r.NextFreeTime = DateTime.Today.AddHours(9);
            }

            // Сортировка заказов по сроку
            var sortedOrders = _orders.OrderBy(o => o.Deadline).ToList();

            foreach (Order order in sortedOrders)
            {
                if (order.Product == null || order.Product.Operations.Count == 0)
                    continue;

                for (int unit = 0; unit < order.Qty; unit++)
                {
                    DateTime currentStart = DateTime.Today.AddHours(9);

                    if (unit == 0 && order.EarliestStart.HasValue && currentStart < order.EarliestStart.Value)
                        currentStart = order.EarliestStart.Value;

                    foreach (Operation op in order.Product.Operations.OrderBy(o => o.Seq))
                    {
                        // Поиск ресурса по точному совпадению типа
                        Resource resource = null;
                        foreach (Resource r in _resources)
                        {
                            if (r.Type == op.NeedResource)
                            {
                                resource = r;
                                break;
                            }
                        }

                        if (resource == null)
                        {
                            // Диагностика
                            MessageBox.Show($"Не найден ресурс типа '{op.NeedResource}' для операции '{op.Name}'",
                                "Ошибка планирования", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        // Время начала
                        DateTime start = currentStart;
                        if (resource.NextFreeTime > start)
                            start = resource.NextFreeTime;

                        // Корректировка рабочего дня
                        if (start.TimeOfDay < new TimeSpan(9, 0, 0))
                            start = start.Date.AddHours(9);
                        if (start.TimeOfDay >= new TimeSpan(18, 0, 0))
                            start = start.Date.AddDays(1).AddHours(9);

                        DateTime end = start.AddHours(op.Hours);

                        // Перенос на следующий день
                        while (end.TimeOfDay > new TimeSpan(18, 0, 0))
                        {
                            double remainingHours = (end - start).TotalHours;
                            start = start.Date.AddDays(1).AddHours(9);
                            end = start.AddHours(remainingHours);
                        }

                        result.Add(new ScheduleItem
                        {
                            Id = nextId++,
                            Start = start,
                            End = end,
                            Resource = resource,
                            Order = order,
                            Operation = op
                        });

                        resource.NextFreeTime = end;
                        currentStart = end;
                    }
                }
            }

            return result;
        }
    }
}