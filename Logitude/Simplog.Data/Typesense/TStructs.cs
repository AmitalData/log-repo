namespace Simplog.Data.Typesense
{

    public class FieldType
    {
        public const string String = "string";
        public const string Int32 = "int32";
        public const string Int64 = "int64";
        public const string Bool = "bool";
    }

    public class SearchResponse<T>
    {
        public object[] facet_counts { get; set; }
        public int found { get; set; }
        public Hit<T>[] hits { get; set; }
        public int out_of { get; set; }
        public int page { get; set; }
        public object request_params { get; set; }
        public bool search_cutoff { get; set; }
        public int search_time_ms { get; set; }
    }

    public class Hit<T>
    {
        public T document { get; set; }
        public object highlight { get; set; }
        public Highlight[] highlights { get; set; }
        public long text_match { get; set; }
        public Text_Match_Info text_match_info { get; set; }
    }

    public class Text_Match_Info
    {
        public string best_field_score { get; set; }
        public int best_field_weight { get; set; }
        public int fields_matched { get; set; }
        public int num_tokens_dropped { get; set; }
        public string score { get; set; }
        public int tokens_matched { get; set; }
        public int typo_prefix_score { get; set; }
    }

    public class Highlight
    {
        public string field { get; set; }
        public string[] matched_tokens { get; set; }
        public string snippet { get; set; }
    }

    public class CreateTableRequest
    {
        public string name { get; set; }
        public Field[] fields { get; set; }
        public string default_sorting_field { get; set; }
    }

    public class Field
    {
        public Field() { }

        public Field(string name, string type, bool facet = false)
        {
            this.name = name;
            this.type = type;
            this.facet = facet;
        }

        public string name { get; set; }
        public string type { get; set; }
        public bool facet { get; set; }
    }
}
