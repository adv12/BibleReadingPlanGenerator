using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;

namespace BibleReadingPlanGeneratorLib
{
    public class PlanSpec
    {

        public string Name { get; set; } = "Bible Reading Plan";

        public int NumberOfDays { get; set; } = 365;

        public List<GroupSpec> Groups { get; set; } = new List<GroupSpec>();

        public PlanSpec()
        {
            
        }

        public PlanSpec(string name, int numberOfDays, IEnumerable<GroupSpec> groups)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Name = name;
            Guard.Against.OutOfRange(numberOfDays, nameof(numberOfDays), 1, int.MaxValue);
            NumberOfDays = numberOfDays;
            Guard.Against.NullOrEmpty(groups, nameof(groups));
            Groups.AddRange(groups);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: ");
            sb.AppendLine(Name);
            sb.Append("Number of days: ");
            sb.AppendLine(NumberOfDays.ToString());
            sb.AppendLine("Groups:");
            foreach (GroupSpec group in Groups)
            {
                sb.AppendLine(group.ToString());
            }
            return sb.ToString();
        }
    }
}
