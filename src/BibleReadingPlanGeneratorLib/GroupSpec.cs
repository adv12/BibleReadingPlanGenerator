using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;

namespace BibleReadingPlanGeneratorLib
{
    public class GroupSpec
    {

        public string Name { get; set; }

        public List<SectionSpec> Sections { get; set; } =
            new List<SectionSpec>();

        public int Repetitions { get; set; } = 1;

        public GroupSpec()
        {

        }

        public GroupSpec(string name, IEnumerable<SectionSpec> sections,
            int repetitions)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Name = name;
            Guard.Against.NullOrEmpty(sections, nameof(sections));
            Sections.AddRange(sections);
            Guard.Against.OutOfRange(repetitions, nameof(repetitions), 1, int.MaxValue);
            Repetitions = repetitions;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: ");
            sb.AppendLine(Name);
            sb.AppendLine("Sections:");
            foreach (SectionSpec section in Sections)
            {
                sb.Append("\t");
                sb.AppendLine(section.ToString());
            }
            sb.Append("Repetitions: ");
            sb.AppendLine(Repetitions.ToString());
            return sb.ToString();
        }

    }
}
