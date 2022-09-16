using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using System;
using System.Linq;
using System.Numerics;

namespace AvaloniaCompositionTest.Views;
public partial class MainWindow : Window
{
    private CompositionVisual? _compositionVisual;
    private Vector3KeyFrameAnimation? _translationAnimation;
    private Vector3KeyFrameAnimation? _pulseAnimation;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();

        CreateAnimations();
    }

    private void CreateAnimations()
    {
        if (_greetingBlock is null)
            return;

        if (ElementComposition.GetElementVisual(_greetingBlock) is not CompositionVisual visual)
            return;

        _compositionVisual = visual;

        CreateTranslationAnimation(visual);
        CreatePulseAnimation(visual);
    }

    private void CreateTranslationAnimation(CompositionVisual visual)
    {
        Compositor compositor = visual.Compositor;
        Vector3KeyFrameAnimation animation = compositor.CreateVector3KeyFrameAnimation();
        var origin = visual.Offset;

        var translations = new Vector3[] { new(200, 0, 0), new(200, 200, 0), new(-200, 200, 0), new(-200, 0, 0) };

        animation.InsertKeyFrame(0f, origin);
        animation.InsertKeyFrame(0.2f, origin + translations[0]);
        animation.InsertKeyFrame(0.4f, origin + translations[1]);
        animation.InsertKeyFrame(0.6f, origin + translations[2]);
        animation.InsertKeyFrame(0.8f, origin + translations[3]);
        animation.InsertKeyFrame(1f, origin);

        animation.Duration = TimeSpan.FromMilliseconds(1500);

        _translationAnimation = animation;
    }

    private void CreatePulseAnimation(CompositionVisual visual)
    {
        Compositor compositor = visual.Compositor;
        Vector3KeyFrameAnimation animation = compositor.CreateVector3KeyFrameAnimation();

        var scales = new float[] { 1f, 4f, 0.25f, 1f }
            .Select(x => new Vector3(x, x, 1f))
            .ToArray();

        animation.InsertKeyFrame(0.0f, scales[0]);
        animation.InsertKeyFrame(0.2f, scales[1]);
        animation.InsertKeyFrame(0.6f, scales[2]);
        animation.InsertKeyFrame(1f, scales[3]);

        animation.Duration = TimeSpan.FromMilliseconds(1500);

        _pulseAnimation = animation;
    }

    public void OnTranslate(object? sender, RoutedEventArgs e)
    {
        if (_compositionVisual is not null && _translationAnimation is not null)
            _compositionVisual.StartAnimation("Offset", _translationAnimation);
    }

    public void OnPulse(object? sender, RoutedEventArgs e)
    {
        if (_compositionVisual is not null && _pulseAnimation is not null)
            _compositionVisual.StartAnimation("Scale", _pulseAnimation);
    }
}
