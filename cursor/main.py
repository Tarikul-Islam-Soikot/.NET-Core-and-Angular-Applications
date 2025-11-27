import subprocess
import time
import pyautogui
import os
from datetime import datetime
import psutil
import win32gui
import win32con
import random
import glob

class CombinedHelper:
    def __init__(self):
        self.backup_data = {}
        self.teams_hwnd = None
        self.vscode_hwnd = None
    
    def find_window_by_title(self, partial_title):
        """Find window handle by partial title"""
        def enum_handler(hwnd, results):
            if win32gui.IsWindowVisible(hwnd):
                window_text = win32gui.GetWindowText(hwnd)
                if partial_title.lower() in window_text.lower():
                    results.append(hwnd)
        
        results = []
        win32gui.EnumWindows(enum_handler, results)
        return results[0] if results else None
    
    def focus_window(self, hwnd):
        """Focus specific window by handle"""
        if hwnd:
            win32gui.ShowWindow(hwnd, win32con.SW_MAXIMIZE)
            win32gui.SetForegroundWindow(hwnd)
            return True
        return False
    
    def open_teams(self):
        """Open Teams and minimize it"""
        try:
            subprocess.Popen('powershell "Start-Process shell:AppsFolder\\MSTeams_8wekyb3d8bbwe!MSTeams"', shell=True)
        except:
            subprocess.Popen('start ms-teams:', shell=True)
        
        # Wait for Teams to open and find its window
        for _ in range(10):  # Try for 10 seconds
            time.sleep(1)
            self.teams_hwnd = self.find_window_by_title("Microsoft Teams")
            if self.teams_hwnd:
                win32gui.ShowWindow(self.teams_hwnd, win32con.SW_MINIMIZE)
                print("Teams opened and minimized")
                break
        else:
            print("Could not find Teams window")
    
    def backup_file(self, file_path):
        """Create backup of current file state"""
        if os.path.exists(file_path):
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            self.backup_data[file_path] = {
                'content': content,
                'timestamp': datetime.now().isoformat()
            }
            print(f"Backup created for {os.path.basename(file_path)}")
            return True
        return False
    
    def revert_changes(self, file_path):
        """Revert file to last backup"""
        if file_path in self.backup_data:
            backup = self.backup_data[file_path]
            
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(backup['content'])
            
            print(f"Reverted {os.path.basename(file_path)} to original state")
            return True
        else:
            print(f"No backup found for {file_path}")
            return False
    
    def open_random_project_file(self):
        """Open a random file from VS Code projects using Quick Open"""
        if self.focus_window(self.vscode_hwnd):
            time.sleep(0.5)
            
            # Use Quick Open to see all files across projects
            pyautogui.hotkey('ctrl', 'p')
            time.sleep(1)
            
            # Search for specific file extensions
            extensions = ['.ts', '.json', '.html', '.scss', '.css', '.js', '.py', '.md']
            extension = random.choice(extensions)
            print(f"Searching for {extension} files...")
            pyautogui.write(extension)
            time.sleep(1)
            
            # Select a random file from the list (press down arrow random times)
            for _ in range(random.randint(0, 4)):
                pyautogui.press('down')
                time.sleep(0.2)
            
            # Open the selected file
            pyautogui.press('enter')
            time.sleep(1)
            
            # Get the opened file path
            return self.get_current_file_path()
        return None
    
    def get_current_file_path(self):
        """Get current file path using VS Code command"""
        if self.focus_window(self.vscode_hwnd):
            time.sleep(0.5)
            pyautogui.hotkey('ctrl', 'shift', 'p')
            time.sleep(0.5)
            pyautogui.write('copy path')
            time.sleep(0.5)
            pyautogui.press('enter')
            time.sleep(0.5)
            
            try:
                import pyperclip
                file_path = pyperclip.paste()
                if os.path.exists(file_path):
                    return file_path
            except:
                pass
        return None
    
    def open_file_from_explorer(self):
        """Open file using VS Code file explorer"""
        if self.focus_window(self.vscode_hwnd):
            time.sleep(0.5)
            
            # Focus on file explorer
            pyautogui.hotkey('ctrl', 'shift', 'e')
            time.sleep(1)
            
            # Navigate through folders (press arrow keys randomly)
            actions = ['down', 'right', 'down', 'down', 'right', 'down']
            for action in actions[:random.randint(2, 4)]:
                pyautogui.press(action)
                time.sleep(0.3)
            
            # Open selected file
            pyautogui.press('enter')
            time.sleep(1)
            
            return self.get_current_file_path()
        return None
    
    def switch_to_next_file_in_vscode(self):
        """Switch to next open file in VS Code"""
        if self.focus_window(self.vscode_hwnd):
            time.sleep(0.5)
            pyautogui.hotkey('ctrl', 'tab')  # Switch to next file
            time.sleep(0.5)
            return True
        return False
    
    def get_project_file(self, cycle_num):
        """Search and open a file from VS Code projects"""
        print(f"Searching for project files by extension...")
        
        # Search and open a file by extension
        current_file = self.open_random_project_file()
        
        if current_file and os.path.exists(current_file):
            print(f"Opened: {os.path.basename(current_file)}")
            return current_file
        else:
            print("Using fallback file")
            return os.path.join(os.getcwd(), 'main.py')
    
    def type_text_slowly(self, text):
        """Type text slowly like human typing"""
        if self.focus_window(self.vscode_hwnd):
            for char in text:
                pyautogui.write(char)
                # Random typing speed between 0.05 to 0.3 seconds per character
                time.sleep(random.uniform(0.05, 0.3))
                
                # Occasional longer pauses (thinking)
                if random.random() < 0.1:  # 10% chance
                    time.sleep(random.uniform(0.5, 1.5))
    

    
    def get_modification_by_type(self, file_path):
        """Get random human-like modification text based on file type"""
        ext = os.path.splitext(file_path)[1].lower()
        
        modifications = {
            '.py': [
                '# TODO: refactor this function later',
                '# FIXME: handle edge case here',
                '# NOTE: this might need optimization',
                '# DEBUG: temporary logging',
                '# HACK: quick fix for now'
            ],
            '.js': [
                '// TODO: add error handling',
                '// FIXME: memory leak issue',
                '// NOTE: consider using async/await',
                '// DEBUG: console log for testing',
                '// REVIEW: optimize this logic'
            ],
            '.ts': [
                '// TODO: add proper type definitions',
                '// FIXME: type assertion needed',
                '// NOTE: interface might change',
                '// DEBUG: temporary any type',
                '// REVIEW: generic implementation'
            ],
            '.html': [
                '<!-- TODO: add accessibility attributes -->',
                '<!-- FIXME: broken layout on mobile -->',
                '<!-- NOTE: update content later -->',
                '<!-- DEBUG: temporary placeholder -->',
                '<!-- REVIEW: semantic HTML needed -->'
            ],
            '.css': [
                '/* TODO: responsive design needed */',
                '/* FIXME: cross-browser compatibility */',
                '/* NOTE: consider CSS Grid instead */',
                '/* DEBUG: temporary styling */',
                '/* REVIEW: optimize selectors */'
            ],
            '.scss': [
                '/* TODO: create mixin for this */',
                '/* FIXME: variable naming convention */',
                '/* NOTE: nested too deep */',
                '/* DEBUG: temporary color values */',
                '/* REVIEW: extract to partial */'
            ],
            '.json': [
                '// TODO: validate schema',
                '// FIXME: missing required fields',
                '// NOTE: structure might change',
                '// DEBUG: test data only',
                '// REVIEW: optimize data structure'
            ],
            '.md': [
                '<!-- TODO: add more examples -->',
                '<!-- FIXME: broken links -->',
                '<!-- NOTE: update documentation -->',
                '<!-- DEBUG: draft content -->',
                '<!-- REVIEW: grammar check needed -->'
            ]
        }
        
        if ext in modifications:
            return random.choice(modifications[ext])
        else:
            generic_comments = [
                '# TODO: implement this feature',
                '# FIXME: bug needs attention',
                '# NOTE: temporary solution',
                '# DEBUG: testing purposes',
                '# REVIEW: code review needed'
            ]
            return random.choice(generic_comments)
    
    def open_file_in_vscode(self, file_path):
        """Open specific file in VS Code"""
        if self.focus_window(self.vscode_hwnd):
            time.sleep(1)
            pyautogui.hotkey('ctrl', 'o')  # Open file dialog
            time.sleep(1)
            pyautogui.write(file_path)
            time.sleep(0.5)
            pyautogui.press('enter')
            time.sleep(1)
            return True
        return False

def run_cycle(helper, cycle_num):
    """Run one complete cycle"""
    print(f"\n=== Cycle {cycle_num} === (Run stop.cmd to stop)")
    
    try:
        # Search and open a file from VS Code projects
        current_file = helper.get_project_file(cycle_num)
        if not current_file:
            print("No file found")
            return
        
        # Backup file
        helper.backup_file(current_file)
        modification = helper.get_modification_by_type(current_file)
        
        # Position cursor at random line in VS Code
        if helper.focus_window(helper.vscode_hwnd):
            time.sleep(1)
            
            # Go to random line
            pyautogui.hotkey('ctrl', 'g')  # Go to line dialog
            time.sleep(0.5)
            
            # Get file line count and go to random line
            with open(current_file, 'r', encoding='utf-8') as f:
                line_count = len(f.readlines())
            
            if line_count > 0:
                random_line = random.randint(1, line_count)
                pyautogui.write(str(random_line))
                pyautogui.press('enter')
                time.sleep(0.5)
                
                # Go to end of line and add new line
                pyautogui.press('end')
                pyautogui.press('enter')
                
                print(f"Typing at line {random_line}: {modification}")
                
                # Type slowly like human
                helper.type_text_slowly(modification)
                
                # Save the file
                time.sleep(1)
                pyautogui.hotkey('ctrl', 's')
                print("Text typed and saved")
            else:
                print("Empty file, typing at beginning")
                helper.type_text_slowly(modification)
                time.sleep(1)
                pyautogui.hotkey('ctrl', 's')
                print("Text typed and saved")
        
        # File is already saved after typing
        
        # Wait before reverting
        wait_time = random.randint(3, 8)
        print(f"Waiting {wait_time} seconds before reverting...")
        time.sleep(wait_time)
        
        # Revert using file backup (simpler and more reliable)
        print("Reverting changes...")
        helper.revert_changes(current_file)
        
        # Reload file in VS Code to show revert
        if helper.focus_window(helper.vscode_hwnd):
            time.sleep(1)
            pyautogui.hotkey('ctrl', 'shift', 'p')
            time.sleep(0.5)
            pyautogui.write('reload')
            time.sleep(0.5)
            pyautogui.press('enter')
            time.sleep(1)
            print("File reverted and reloaded")
        
        print(f"Cycle {cycle_num} completed.")
    
    except Exception as e:
        print(f"Error in cycle {cycle_num}: {e}")
        print("Skipping to next cycle...")

def main():
    helper = CombinedHelper()
    
    print("Auto Helper Starting...")
    
    # Initial setup
    helper.open_teams()
    helper.vscode_hwnd = helper.find_window_by_title("Visual Studio Code")
    
    if not helper.vscode_hwnd:
        print("VS Code not found. Please open VS Code first.")
        return
    
    # Auto-run unlimited cycles with timing
    print("\nRunning UNLIMITED cycles with random timing:")
    print("- Teams opens randomly between 5-30 minutes")
    print("- VS Code work randomly between 30 seconds-2 minutes")
    print("To stop: double-click stop.cmd")
    
    cycle = 1
    last_teams_open = time.time()
    teams_interval = random.randint(300, 1800)  # 5 to 30 minutes (300-1800 seconds)
    print(f"Next Teams opening in {teams_interval//60} minutes {teams_interval%60} seconds")
    
    while True:
        try:
            # Check if random time passed - open Teams
            current_time = time.time()
            if current_time - last_teams_open >= teams_interval:
                print(f"\n--- Opening Teams (after {teams_interval//60}m {teams_interval%60}s) ---")
                helper.open_teams()
                last_teams_open = current_time
                # Set next random interval
                teams_interval = random.randint(300, 1800)
                print(f"Next Teams opening in {teams_interval//60} minutes {teams_interval%60} seconds")
            
            # Run VS Code cycle
            run_cycle(helper, cycle)
            cycle += 1
            
            # Wait random time between 30 seconds to 2 minutes
            wait_time = random.randint(30, 120)  # 30 to 120 seconds (2 minutes)
            print(f"Waiting {wait_time} seconds until next VS Code cycle...")
            time.sleep(wait_time)
            
        except KeyboardInterrupt:
            print("\nProgram interrupted by user")
            break
        except Exception as e:
            print(f"Error in cycle {cycle}: {e}")
            print("Continuing to next cycle...")
            cycle += 1
            wait_time = random.randint(30, 120)
            print(f"Error occurred, waiting {wait_time} seconds...")
            time.sleep(wait_time)

if __name__ == "__main__":
    main()
