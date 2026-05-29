using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Attach to your Canvas. Assign BulletEmitter in inspector.
/// Resources/BulletHell/Patterns  → SO_BulletPattern
/// Resources/BulletHell/Waves     → SO_BulletWave
/// Resources/BulletHell/Presets   → SO_BulletPreset
/// </summary>
public class BulletHellDropdownUI : MonoBehaviour
{
    [Header("References")]
    public BulletEmitter emitter;

    // ── Data ──────────────────────────────────────────────────────────────────

    private List<SO_BulletPattern> _patterns = new List<SO_BulletPattern>();
    private List<SO_BulletWave> _waves = new List<SO_BulletWave>();
    private List<SO_BulletPreset> _presets = new List<SO_BulletPreset>();

    // ── UI refs ───────────────────────────────────────────────────────────────

    private TMP_Text _patternLabel;
    private TMP_Text _waveLabel;
    private TMP_Text _presetLabel;

    // ── Style ─────────────────────────────────────────────────────────────────

    static readonly Color C_BG = new Color(0.08f, 0.08f, 0.10f, 0.97f);
    static readonly Color C_HEADER = new Color(0.16f, 0.16f, 0.20f, 1.00f);
    static readonly Color C_HOVER = new Color(0.20f, 0.20f, 0.26f, 1.00f);
    static readonly Color C_ACCENT = new Color(0.25f, 0.55f, 1.00f, 1.00f);
    static readonly Color C_TEXT = new Color(0.90f, 0.90f, 0.95f, 1.00f);
    static readonly Color C_DIM = new Color(0.50f, 0.50f, 0.55f, 1.00f);
    static readonly Color C_DIV = new Color(0.22f, 0.22f, 0.27f, 1.00f);

    const float W = 300f;
    const float HDR_H = 38f;
    const float ROW_H = 34f;
    const float LIST_H = 220f;

    // ─────────────────────────────────────────────────────────────────────────

    void Start()
    {
        LoadAssets();
        BuildUI();
    }

    // ── Load ──────────────────────────────────────────────────────────────────

    void LoadAssets()
    {
        _patterns = new List<SO_BulletPattern>(Resources.LoadAll<SO_BulletPattern>("BulletHell/Patterns"));
        _waves = new List<SO_BulletWave>(Resources.LoadAll<SO_BulletWave>("BulletHell/Waves"));
        _presets = new List<SO_BulletPreset>(Resources.LoadAll<SO_BulletPreset>("BulletHell/Presets"));
        _patterns.Sort((a, b) => string.Compare(a.name, b.name));
        _waves.Sort((a, b) => string.Compare(a.name, b.name));
        _presets.Sort((a, b) => string.Compare(a.name, b.name));
        Debug.Log($"[BulletHellUI] {_patterns.Count} patterns, {_waves.Count} waves, {_presets.Count} presets");
    }

    // ── Build ─────────────────────────────────────────────────────────────────

    void BuildUI()
    {
        var root = NewRect("BH_Root", transform);
        SetAnchors(root, new Vector2(0, 1), new Vector2(0, 1));
        root.pivot = new Vector2(0, 1);
        root.anchoredPosition = new Vector2(12, -12);
        root.sizeDelta = new Vector2(W, 0);

        root.gameObject.AddComponent<Image>().color = C_BG;

        var vlg = root.gameObject.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.spacing = 1;

        var csf = root.gameObject.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // ── PATTERN ───────────────────────────────────────────────────────────
        BuildSection(root, "PATTERN", _patterns.Count,
            out _patternLabel,
            ROW_H,
            buildRow: (content, i) =>
            {
                string name = _patterns[i].name.Replace("Pattern_", "");
                BuildSimpleRow(content, i, name, () => SelectPattern(i));
            });

        Divider(root);

        // ── WAVE ──────────────────────────────────────────────────────────────
        BuildSection(root, "WAVE", _waves.Count,
            out _waveLabel,
            ROW_H,
            buildRow: (content, i) =>
            {
                string name = _waves[i].name.Replace("Wave_", "");
                BuildSimpleRow(content, i, name, () => SelectWave(i));
            });

        Divider(root);

        // ── PRESET ────────────────────────────────────────────────────────────
        BuildSection(root, "PRESET", _presets.Count,
            out _presetLabel,
            ROW_H + 14f,
            buildRow: (content, i) =>
            {
                var preset = _presets[i];
                string name = preset.name.Replace("Preset_", "");
                string wName = preset.wave != null ? preset.wave.name.Replace("Wave_", "") : "?";
                string pName = preset.pattern != null ? preset.pattern.name.Replace("Pattern_", "") : "?";
                BuildPresetRow(content, i, name, $"{wName}  +  {pName}", () => SelectPreset(i));
            });
    }

    // ── Shared section builder ────────────────────────────────────────────────

    void BuildSection(RectTransform parent, string title, int count,
                      out TMP_Text labelOut,
                      float rowHeight,
                      System.Action<Transform, int> buildRow)
    {
        // Wrapper — starts at HDR_H, expands to HDR_H+LIST_H when open
        var section = NewRect($"Sec_{title}", parent);
        var secLE = AddLE(section.gameObject, W, HDR_H);

        // Header
        var hdr = NewRect("Header", section);
        var hdrImg = AddImage(hdr, C_HEADER);
        AddLE(hdr.gameObject, W, HDR_H);
        SetAnchors(hdr, new Vector2(0, 1), new Vector2(1, 1));
        hdr.pivot = new Vector2(0.5f, 1);
        hdr.anchoredPosition = Vector2.zero;
        hdr.sizeDelta = new Vector2(0, HDR_H);

        var btn = hdr.gameObject.AddComponent<Button>();
        var bc = btn.colors;
        bc.normalColor = C_HEADER;
        bc.highlightedColor = new Color(0.22f, 0.22f, 0.28f, 1f);
        bc.pressedColor = new Color(0.11f, 0.11f, 0.15f, 1f);
        btn.colors = bc;
        btn.targetGraphic = hdrImg;

        // Arrow
        var arrowGO = NewRect("Arrow", hdr);
        SetAnchors(arrowGO, new Vector2(0, 0), new Vector2(0, 1));
        arrowGO.pivot = new Vector2(0, 0.5f);
        arrowGO.anchoredPosition = new Vector2(10, 0);
        arrowGO.sizeDelta = new Vector2(16, 0);
        var arrow = AddText(arrowGO, "▶", 10, C_DIM, TextAlignmentOptions.Center);

        // Title label
        var titleGO = NewRect("Title", hdr);
        SetAnchors(titleGO, new Vector2(0, 0), new Vector2(0.5f, 1));
        titleGO.offsetMin = new Vector2(32, 0);
        titleGO.offsetMax = Vector2.zero;
        AddText(titleGO, title, 10, C_DIM, TextAlignmentOptions.MidlineLeft);

        // Selected value
        var valGO = NewRect("Value", hdr);
        SetAnchors(valGO, new Vector2(0, 0), new Vector2(1, 1));
        valGO.offsetMin = Vector2.zero;
        valGO.offsetMax = new Vector2(-10, 0);
        labelOut = AddText(valGO, "none", 12, C_TEXT, TextAlignmentOptions.MidlineRight);

        // List panel — fills section rect below the header
        var list = NewRect("List", section);
        var listImg = AddImage(list, new Color(0.10f, 0.10f, 0.13f, 1f));
        list.anchorMin = new Vector2(0, 0);
        list.anchorMax = new Vector2(1, 1);
        list.offsetMin = new Vector2(0, 0);
        list.offsetMax = new Vector2(0, -HDR_H);
        list.gameObject.SetActive(false);

        // ScrollRect
        var sr = list.gameObject.AddComponent<ScrollRect>();
        sr.horizontal = false;

        var viewport = NewRect("Viewport", list);
        SetAnchors(viewport, Vector2.zero, Vector2.one);
        viewport.offsetMin = Vector2.zero;
        viewport.offsetMax = new Vector2(-10, 0);
        viewport.gameObject.AddComponent<RectMask2D>();
        sr.viewport = viewport;

        var content = NewRect("Content", viewport);
        SetAnchors(content, new Vector2(0, 1), new Vector2(1, 1));
        content.pivot = new Vector2(0.5f, 1);
        content.sizeDelta = new Vector2(0, count * rowHeight);
        var cVLG = content.gameObject.AddComponent<VerticalLayoutGroup>();
        cVLG.childControlWidth = true; cVLG.childControlHeight = true;
        cVLG.childForceExpandWidth = true; cVLG.childForceExpandHeight = false;
        sr.content = content;

        // Scrollbar
        var sbGO = NewRect("Scrollbar", list);
        SetAnchors(sbGO, new Vector2(1, 0), new Vector2(1, 1));
        sbGO.pivot = new Vector2(1, 0.5f);
        sbGO.anchoredPosition = Vector2.zero;
        sbGO.sizeDelta = new Vector2(8, 0);
        AddImage(sbGO, new Color(0.14f, 0.14f, 0.17f, 1f));
        var sb = sbGO.gameObject.AddComponent<Scrollbar>();
        sb.direction = Scrollbar.Direction.BottomToTop;
        var sliding = NewRect("Sliding Area", sbGO);
        SetAnchors(sliding, Vector2.zero, Vector2.one); sliding.sizeDelta = Vector2.zero;
        var handle = NewRect("Handle", sliding);
        SetAnchors(handle, Vector2.zero, Vector2.one); handle.sizeDelta = Vector2.zero;
        AddImage(handle, new Color(0.32f, 0.32f, 0.42f, 1f));
        sb.handleRect = handle;
        sr.verticalScrollbar = sb;
        sr.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;

        // Populate rows via delegate
        for (int i = 0; i < count; i++)
            buildRow(content, i);

        // Toggle
        GameObject capList = list.gameObject;
        TMP_Text capArrow = arrow;
        LayoutElement capLE = secLE;

        btn.onClick.AddListener(() =>
        {
            bool open = !capList.activeSelf;
            capList.SetActive(open);
            capArrow.text = open ? "▼" : "▶";
            capLE.preferredHeight = open ? HDR_H + LIST_H : HDR_H;
            capLE.minHeight = open ? HDR_H + LIST_H : HDR_H;
        });
    }

    // ── Row builders ──────────────────────────────────────────────────────────

    void BuildSimpleRow(Transform content, int idx, string label, System.Action onClick)
    {
        var row = NewRect($"Row_{idx}", content);
        AddLE(row.gameObject, W, ROW_H);
        var rowImg = AddImage(row, Color.clear);

        var btn = row.gameObject.AddComponent<Button>();
        var rc = btn.colors;
        rc.normalColor = Color.clear;
        rc.highlightedColor = C_HOVER;
        rc.pressedColor = new Color(C_ACCENT.r, C_ACCENT.g, C_ACCENT.b, 0.4f);
        rc.fadeDuration = 0.05f;
        btn.colors = rc;
        btn.targetGraphic = rowImg;
        btn.onClick.AddListener(() => onClick());

        var accent = NewRect("Accent", row);
        SetAnchors(accent, new Vector2(0, 0.1f), new Vector2(0, 0.9f));
        accent.sizeDelta = new Vector2(3, 0);
        AddImage(accent, C_ACCENT);
        accent.gameObject.SetActive(false);

        var lbl = NewRect("Label", row);
        SetAnchors(lbl, Vector2.zero, Vector2.one);
        lbl.offsetMin = new Vector2(14, 0);
        lbl.offsetMax = Vector2.zero;
        AddText(lbl, label, 12, C_TEXT, TextAlignmentOptions.MidlineLeft);
    }

    void BuildPresetRow(Transform content, int idx, string name, string sub, System.Action onClick)
    {
        var row = NewRect($"Row_{idx}", content);
        AddLE(row.gameObject, W, ROW_H + 14f); // slightly taller for subtitle
        var rowImg = AddImage(row, Color.clear);

        var btn = row.gameObject.AddComponent<Button>();
        var rc = btn.colors;
        rc.normalColor = Color.clear;
        rc.highlightedColor = new Color(0.16f, 0.24f, 0.30f, 1f);
        rc.pressedColor = new Color(C_ACCENT.r, C_ACCENT.g, C_ACCENT.b, 0.3f);
        rc.fadeDuration = 0.05f;
        btn.colors = rc;
        btn.targetGraphic = rowImg;
        btn.onClick.AddListener(() => onClick());

        var accent = NewRect("Accent", row);
        SetAnchors(accent, new Vector2(0, 0.1f), new Vector2(0, 0.9f));
        accent.sizeDelta = new Vector2(3, 0);
        AddImage(accent, new Color(0.35f, 0.65f, 1f, 1f));
        accent.gameObject.SetActive(false);

        // Two-line column: name + subtitle
        var col = NewRect("Col", row);
        SetAnchors(col, Vector2.zero, Vector2.one);
        col.offsetMin = new Vector2(14, 2); col.offsetMax = new Vector2(0, -2);
        var colVLG = col.gameObject.AddComponent<VerticalLayoutGroup>();
        colVLG.childControlWidth = true; colVLG.childControlHeight = true;
        colVLG.childForceExpandWidth = true; colVLG.childForceExpandHeight = true;

        var nameGO = NewRect("Name", col);
        AddText(nameGO, name, 12, C_TEXT, TextAlignmentOptions.MidlineLeft);

        var subGO = NewRect("Sub", col);
        AddText(subGO, sub, 9, C_DIM, TextAlignmentOptions.MidlineLeft);
    }

    // ── Select ────────────────────────────────────────────────────────────────

    void SelectPattern(int idx)
    {
        if (!Check() || idx < 0 || idx >= _patterns.Count) return;
        emitter.pattern = _patterns[idx];
        _patternLabel.text = _patterns[idx].name.Replace("Pattern_", "");
        Highlight(_patternLabel, idx);
    }

    void SelectWave(int idx)
    {
        if (!Check() || idx < 0 || idx >= _waves.Count) return;
        emitter.wave = _waves[idx];
        _waveLabel.text = _waves[idx].name.Replace("Wave_", "");
        Highlight(_waveLabel, idx);
    }

    void SelectPreset(int idx)
    {
        if (!Check() || idx < 0 || idx >= _presets.Count) return;
        var p = _presets[idx];

        // Apply wave + pattern, mirror into their dropdowns
        if (p.wave != null)
        {
            emitter.wave = p.wave;
            _waveLabel.text = p.wave.name.Replace("Wave_", "");
            int wi = _waves.IndexOf(p.wave);
            if (wi >= 0) HighlightInList("Sec_WAVE", wi);
        }
        if (p.pattern != null)
        {
            emitter.pattern = p.pattern;
            _patternLabel.text = p.pattern.name.Replace("Pattern_", "");
            int pi = _patterns.IndexOf(p.pattern);
            if (pi >= 0) HighlightInList("Sec_PATTERN", pi);
        }

        _presetLabel.text = p.name.Replace("Preset_", "");
        HighlightInList("Sec_PRESET", idx);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    bool Check()
    {
        if (emitter != null) return true;
        Debug.LogWarning("No emitter assigned."); return false;
    }

    // Highlight by walking up from the label to find its section list
    void Highlight(TMP_Text sectionLabel, int selectedIdx)
    {
        // Label lives in Header > Section, list sibling is "List"
        var list = sectionLabel.transform.parent.parent.parent.Find("List");
        if (list == null) return;
        HighlightList(list, selectedIdx);
    }

    void HighlightInList(string sectionName, int selectedIdx)
    {
        var root = transform.Find($"BH_Root/{sectionName}");
        if (root == null) return;
        var list = root.Find("List");
        if (list == null) return;
        HighlightList(list, selectedIdx);
    }

    static void HighlightList(Transform list, int selectedIdx)
    {
        var content = list.Find("Viewport/Content");
        if (content == null) return;
        for (int i = 0; i < content.childCount; i++)
        {
            var accent = content.GetChild(i).Find("Accent");
            if (accent != null) accent.gameObject.SetActive(i == selectedIdx);
        }
    }

    // ── Factory ───────────────────────────────────────────────────────────────

    static RectTransform NewRect(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go.GetComponent<RectTransform>();
    }

    static void SetAnchors(RectTransform rt, Vector2 min, Vector2 max)
    {
        rt.anchorMin = min; rt.anchorMax = max;
        rt.anchoredPosition = Vector2.zero; rt.sizeDelta = Vector2.zero;
    }

    static Image AddImage(RectTransform rt, Color color)
    {
        var img = rt.gameObject.AddComponent<Image>();
        img.color = color; return img;
    }

    static TMP_Text AddText(RectTransform rt, string value, int size, Color color, TextAlignmentOptions align)
    {
        var t = rt.gameObject.AddComponent<TextMeshProUGUI>();
        t.text = value; t.fontSize = size; t.color = color;
        t.alignment = align; t.overflowMode = TextOverflowModes.Ellipsis;
        return t;
    }

    static LayoutElement AddLE(GameObject go, float w, float h)
    {
        var le = go.AddComponent<LayoutElement>();
        le.preferredWidth = w; le.preferredHeight = h; le.minHeight = h;
        return le;
    }

    void Divider(RectTransform parent)
    {
        var d = NewRect("Div", parent);
        d.gameObject.AddComponent<Image>().color = C_DIV;
        AddLE(d.gameObject, W, 1f);
    }
}