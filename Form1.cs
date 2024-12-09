namespace servecoin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddTargetsToTable();
        }
    }
}
