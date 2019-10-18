namespace AndroidGoodiesExamples
{
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using UnityEngine;
	using UnityEngine.UI;
#if UNITY_ANDROID
	using DeadMosquito.AndroidGoodies;
	using DeadMosquito.AndroidGoodies.Internal;

#endif
	public class ShortcutsTest : MonoBehaviour
	{
		const string ShortcutId = "shortcut_id";
		
		[SerializeField]
		Texture2D _icon;

		[SerializeField]
		Text _resultText;
#if UNITY_ANDROID
		void Start()
		{
			if (AGShortcutManager.GetShortcutParameter(ShortcutId) != null)
			{
				_resultText.text = AGShortcutManager.GetShortcutParameter(ShortcutId);
			}
		}

		[UsedImplicitly]
		public void CreateWikiShortcut()
		{
			PublishShortcut(CreateDocsShortcut());
		}

		[UsedImplicitly]
		public void CreatePinnedShortcut()
		{
			AGShortcutManager.RequestPinShortcut(CreateDocsShortcut());
		}

		ShortcutInfo CreateDocsShortcut()
		{
			return CreateBasicShortcut()
				.SetShortLabel("Documentation")
				.SetLongLabel("Read the documentation")
				.SetOpenUrlIntent("https://github.com/NinevaStudios/android-goodies-docs-PRO")
				.Build();
		}

		[UsedImplicitly]
		public void CreateThisAppShortcut()
		{
			var customData = new Dictionary<string, string> {{ShortcutId, "App was opened via Shortcut"}};
			var shortcut = CreateBasicShortcut()
				.SetShortLabel("Open App")
				.SetOpenThisAppIntent(customData)
				.Build();

			PublishShortcut(shortcut);
		}

		[UsedImplicitly]
		public void DisablePinnedShortcut()
		{
			var idList = new List<string> {ShortcutId};
			AGShortcutManager.DisableShortcuts(idList, "Disabled");
		}

		[UsedImplicitly]
		public void EnablePinnedShortcut()
		{
			var idList = new List<string> {ShortcutId};
			AGShortcutManager.EnableShortcuts(idList);
		}

		[UsedImplicitly]
		public void RemoveAllDynamicShortcuts()
		{
			AGShortcutManager.RemoveAllDynamicShortcuts();
		}

		[UsedImplicitly]
		public void RemoveDynamicShortcut()
		{
			var idList = new List<string> {ShortcutId};
			AGShortcutManager.RemoveDynamicShortcuts(idList);
		}

		[UsedImplicitly]
		public void GetAllShortcuts()
		{
			var dynamicList = AGShortcutManager.DynamicShortcuts;
			var pinnedList = AGShortcutManager.PinnedShortcuts;
			_resultText.text = "Pinned Shortcuts:\n";
			foreach (var shortcut in pinnedList)
			{
				_resultText.text += shortcut + "\n";
			}

			_resultText.text += "\nDynamic Shortcuts:\n";
			foreach (var shortcut in dynamicList)
			{
				_resultText.text += shortcut + "\n";
			}
		}

		// TODO move wifi staff to some other panel, wifi and shortcuts do not belong to same logical group
		ShortcutInfo.Builder CreateBasicShortcut()
		{
			var builder = new ShortcutInfo.Builder(ShortcutId)
				.SetIcon(_icon)
				.SetRank(1)
				.SetLongLabel("Long Label");
			return builder;
		}

		void PublishShortcut(ShortcutInfo shortcut)
		{
			var list = new List<ShortcutInfo> {shortcut};

			AGShortcutManager.AddDynamicShortcuts(list);
			_resultText.text = shortcut.ToString();
		}
#endif
	}
}