using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBindToObservableCollectionWpf
{
    public static class DataProvider
    {
        public static void AddTestData(ObservableCollection<ModelResource> resources, ObservableCollection<ModelAppointment> appointments)
        {
            ModelResource res1 = new ModelResource()
            {
                Id = 0,
                Name = "Computer1",
                Color = ToRgb(System.Drawing.Color.Yellow)
            };

            ModelResource res2 = new ModelResource()
            {
                Id = 1,
                Name = "Computer2",
                Color = ToRgb(System.Drawing.Color.Green)
            };

            ModelResource res3 = new ModelResource()
            {
                Id = 2,
                Name = "Computer3",
                Color = ToRgb(System.Drawing.Color.Blue)
            };

            resources.Add(res1);
            resources.Add(res2);
            resources.Add(res3);

            DateTime baseTime = DateTime.Now;

            ModelAppointment apt1 = new ModelAppointment()
            {
                Id = Guid.NewGuid(),
                StartTime = baseTime.AddHours(-2),
                EndTime = baseTime.AddHours(-1),
                Subject = "Test 1",
                Location = "Office",
                Description = "Test procedure 1",
                Price = 10m,
                ResourceId = 0
            };

            ModelAppointment apt2 = new ModelAppointment()
            {
                Id = Guid.NewGuid(),
                StartTime = baseTime,
                EndTime = baseTime.AddHours(2),
                Subject = "Test 2",
                Location = "Office",
                Description = "Test procedure 2",
                ResourceId = 1
            };

            ModelAppointment apt3 = new ModelAppointment()
            {
                Id = Guid.NewGuid(),
                StartTime = baseTime.AddHours(-1),
                EndTime = baseTime.AddHours(1),
                Subject = "Test 3",
                Location = "Office",
                Description = "Test procedure 3",
                ResourceId = 2
            };

            appointments.Add(apt1);
            appointments.Add(apt2);
            appointments.Add(apt3);
        }
        private static int ToRgb(System.Drawing.Color color)
        {
            return color.B << 16 | color.G << 8 | color.R;
        }
    }
}
