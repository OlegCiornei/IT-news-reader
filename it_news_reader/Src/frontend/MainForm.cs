using System.Collections;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace it_news_reader;

public partial class MainForm : Form
{
    private PictureBox pictureBox;
    private ListBox entityList;
    private TextBox entityText;
    private PictureBox entityImage;
    private Panel leftPanel;
    private StatusStrip statusBar;
    private ToolStripStatusLabel timeStatusLabel;
    private System.Windows.Forms.Timer timer;
    private PictureBox leftImageBox;
    private TextBox ArticleName;
    private TextBox ArticleText;
    private Dictionary<int, int> articlesOrderId;
    private FlowLayoutPanel flowLayoutPanel;
    private Rectangle Bound; 
    public MainForm()
    {
        //InitializeComponent();
        BackColor = Color.LightBlue;
        WindowState = FormWindowState.Maximized;
        InitializeUI();
    }

    private void InitializeUI()
    {
        Bound = Screen.FromControl(this).Bounds;
        InitializeStatusBar();
        InitializeFlowLayotDesign();
        InitializeTimer();
        InitializeLeftPanel();
        InitializeMenu();
    }
    private void InitializeFlowLayotDesign()
    {
        flowLayoutPanel = new FlowLayoutPanel
        {
            AutoScroll = true,
            WrapContents = true,
            Location = new Point(Bound.Width / 100 * 20, Bound.Height / 100 * 10),
            Size = new Size(Bound.Width / 100 * 75, Bound.Height),
        };
        var query = new QueryBuilder().Select("title, link, image, text")
                                          .From("articles")
                                          .Get();
        var result = new SqliteManager().Select(query);
        foreach (DataRow row in result.Rows)
        {
            try { 
                var entity = new EntityControl
                (
                    row["title"].ToString(),
                    row["link"].ToString(),
                    row.IsNull("image") ? null : row["image"].ToString(),
                    row["text"].ToString(),
                    Bound.Width / 100 * 25,
                    Bound.Width / 100 * 95,
                    Bound.Height / 100 * 10,
                    Bound.Width / 100 * 50
                );

                flowLayoutPanel.Controls.Add(entity);
            } catch { 
                continue;
            }
        }
        Controls.Add(flowLayoutPanel);
    }
    private void InitializeMenu()
    {
        MenuStrip menuStrip = new MenuStrip();
        menuStrip.BackColor = Color.Blue;
        ToolStripMenuItem refreshMenuItem = new ToolStripMenuItem("Refresh");
        ToolStripMenuItem siteMenuItem = new ToolStripMenuItem("Site");
        ToolStripMenuItem newestMenuItem = new ToolStripMenuItem("Newest");
        ToolStripMenuItem oldestMenuItem = new ToolStripMenuItem("Oldest");

        menuStrip.Items.AddRange(new ToolStripItem[] {refreshMenuItem, siteMenuItem, newestMenuItem, oldestMenuItem });
        MainMenuStrip = menuStrip;
        Controls.Add(menuStrip);

    }
    private void InitializeLeftPanel()
    {
        var bounds = Screen.FromControl(this).Bounds;
        leftPanel = new Panel
        {
            BackColor = Color.LightBlue,
            Location = new Point(0, 30),
            Size = new Size(bounds.Width / 100 * 20, bounds.Height),
            AutoScroll = true,
            BorderStyle = BorderStyle.None,
        };
        Controls.Add(leftPanel);

        entityList = new ListBox
        {
            BackColor = Color.FromArgb(255, 133, 176, 240),
            Size = new Size(leftPanel.Width, leftPanel.Height),
        };
        var query = new QueryBuilder().Select("id, title")
                .From("articles")
                .Get();
        var result = new SqliteManager().Select(query);
        articlesOrderId = new Dictionary<int, int>();
        for(int i = 0; i < result.Rows.Count; ++i)
        {
            entityList.Items.Add(result.Rows[i]["title"]);
            articlesOrderId.Add(i, Int32.Parse(result.Rows[i]["id"].ToString()));
        }
        entityList.SelectedIndexChanged += EntityList_SelectedIndexChanged;
        leftPanel.Controls.Add(entityList);
    }
    private void InitializeTimer()
    {
        timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000;
        timer.Tick += Timer_Tick;
        timer.Start();
        UpdateTime();
    }
    private void InitializeStatusBar()
    {
        // StatusStrip
        statusBar = new StatusStrip
        {
            Dock = DockStyle.Bottom,
        };
        Controls.Add(statusBar);

        // ToolStripStatusLabel for time
        timeStatusLabel = new ToolStripStatusLabel
        {
            Dock = DockStyle.Right,
            Text = "TM: ",
        };
        statusBar.Items.Add(timeStatusLabel);
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        UpdateTime();
    }
    private void UpdateTime()
    {
        timeStatusLabel.Text = "TM: " + DateTime.Now.ToString("HH:mm:ss");
    }
    private void EntityList_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Handle the selection change
        int selectedIndex = entityList.SelectedIndex;

        if (selectedIndex >= 0)
        {
            var query = new QueryBuilder().Select("title, link, text, image")
                                          .From("articles")
                                          .AndWhere(new Where("id", $"{articlesOrderId[selectedIndex]}"))
                                          .Get();
            var result = new SqliteManager().Select(query);
            if (result.Rows.Count > 0) { 
                flowLayoutPanel.Controls.Clear();
                var entity = new EntityControl
                (
                    result.Rows[0]["title"].ToString(),
                    result.Rows[0]["link"].ToString(),
                    result.Rows[0].IsNull("image") ? null : result.Rows[0]["image"].ToString(),
                    result.Rows[0]["text"].ToString(),
                    Bound.Width / 100 * 25,
                    Bound.Width / 100 * 95,
                    Bound.Height / 100 * 10,
                    Bound.Width / 100 * 90
                );

                flowLayoutPanel.Controls.Add(entity);
              
            }
        }
    }
 }