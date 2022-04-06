using Kasi_Server.Utils.Extensions;
using System.Collections.Concurrent;

namespace Kasi_Server.Utils.IO
{
    public class FileWatcher
    {
        public delegate void FileWatcherEventHandler(object sender, FileWatcherEventArgs args);

        public FileWatcherEventHandler EventHandler;

        private int _interval = 10 * 1000;

        public bool IsWatching { get; private set; } = false;

        private ConcurrentDictionary<string, FileSystemWatcher> _watchers =
            new ConcurrentDictionary<string, FileSystemWatcher>();

        public FileWatcher(string[] paths)
        {
            if (paths.IsEmpty())
            {
                return;
            }
            foreach (var path in paths)
            {
                AddPath(path);
            }
        }

        public bool AddPath(string path)
        {
            if (Directory.Exists(path) && !_watchers.ContainsKey(path))
            {
                return _watchers.TryAdd(path, null);
            }

            return false;
        }

        public bool DeletePath(string path)
        {
            if (_watchers.ContainsKey(path))
            {
                if (_watchers.TryRemove(path, out var temp) && temp != null)
                {
                    temp.EnableRaisingEvents = false;
                    temp.Dispose();
                    return true;
                }
            }

            return false;
        }

        public void Start()
        {
            if (IsWatching)
            {
                return;
            }

            IsWatching = true;
            Task.Factory.StartNew(() =>
            {
                while (IsWatching)
                {
                    foreach (var watcher in _watchers)
                    {
                        if (watcher.Value == null)
                        {
                            if (Directory.Exists(watcher.Key))
                            {
                                _watchers.TryUpdate(watcher.Key, CreateWatcher(watcher.Key), null);
                            }
                        }
                        else
                        {
                            if (!Directory.Exists(watcher.Key))
                            {
                                watcher.Value.EnableRaisingEvents = false;
                                watcher.Value.Dispose();
                                _watchers.TryUpdate(watcher.Key, null, watcher.Value);
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(_interval);
                }

                IsWatching = false;
            });
        }

        public void Stop()
        {
            IsWatching = false;
            foreach (var item in _watchers.Keys)
            {
                if (_watchers.ContainsKey(item))
                {
                    if (_watchers[item] != null)
                    {
                        _watchers[item].EnableRaisingEvents = false;
                    }

                    if (_watchers.TryRemove(item, out var temp) && temp != null)
                    {
                        temp.EnableRaisingEvents = false;
                        temp.Dispose();
                    }
                }
            }
        }

        private FileSystemWatcher CreateWatcher(string path)
        {
            var fsw = new FileSystemWatcher(path);
            fsw.Created += (sender, e) =>
            {
                EventHandler?.Invoke(sender,
                    new FileWatcherEventArgs(e.ChangeType, e.FullPath, Path.GetFileName(e.FullPath), null, null));
            };
            fsw.Changed += (sender, e) =>
            {
                EventHandler?.Invoke(sender,
                    new FileWatcherEventArgs(e.ChangeType, e.FullPath, Path.GetFileName(e.FullPath), null, null));
            };
            fsw.Deleted += (sender, e) =>
            {
                EventHandler?.Invoke(sender,
                    new FileWatcherEventArgs(e.ChangeType, e.FullPath, Path.GetFileName(e.FullPath), null, null));
            };
            fsw.Renamed += (sender, e) =>
            {
                EventHandler?.Invoke(sender,
                    new FileWatcherEventArgs(e.ChangeType, e.FullPath, Path.GetFileName(e.FullPath), e.OldFullPath,
                        e.OldName));
            };
            fsw.Error += (sender, e) => { };
            fsw.IncludeSubdirectories = true;
            fsw.NotifyFilter = (NotifyFilters)383;
            fsw.EnableRaisingEvents = true;
            return fsw;
        }
    }

    public class FileWatcherEventArgs
    {
        public WatcherChangeTypes ChangeTypes { get; }

        public string FullPath { get; }

        public string Name { get; }

        public string OldFullPath { get; }

        public string OldName { get; }

        public FileWatcherEventArgs(WatcherChangeTypes type, string fullPath, string name, string oldFullPath,
            string oldName)
        {
            ChangeTypes = type;
            FullPath = fullPath;
            Name = name;
            OldFullPath = oldFullPath;
            OldName = oldName;
        }
    }
}