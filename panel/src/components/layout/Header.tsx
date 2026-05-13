import { LogOut, ChevronDown } from 'lucide-react';
import { useState, useRef, useEffect } from 'react';
import { useAuth } from '@/hooks/useAuth';
import { cn } from '@/lib/utils';

export function Header() {
  const { user, logout } = useAuth();
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  // Close dropdown when clicking outside
  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsDropdownOpen(false);
      }
    }

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const userInitials = user?.email
    ? user.email
        .split('@')[0]
        .split('.')
        .map((n) => n[0])
        .join('')
        .toUpperCase()
        .slice(0, 2)
    : 'U';

  return (
    <header className="h-16 bg-white border-b border-slate-200 flex items-center justify-end px-6 sticky top-0 z-30">
      {/* Right side - User menu */}
      <div className="relative" ref={dropdownRef}>
        <button
          onClick={() => setIsDropdownOpen(!isDropdownOpen)}
          className="flex items-center gap-3 p-2 hover:bg-slate-100 rounded-md transition-colors"
        >
          <div className="w-8 h-8 bg-sky-100 text-sky-700 rounded-full flex items-center justify-center text-sm font-medium">
            {userInitials}
          </div>
          <div className="hidden sm:block text-left">
            <p className="text-sm font-medium text-slate-700">{user?.email}</p>
            <p className="text-xs text-slate-500">{user?.roles.join(', ')}</p>
          </div>
          <ChevronDown
            className={cn(
              'w-4 h-4 text-slate-400 transition-transform hidden sm:block',
              isDropdownOpen && 'rotate-180'
            )}
          />
        </button>

        {/* Dropdown */}
        {isDropdownOpen && (
          <div className="absolute right-0 mt-2 w-56 bg-white rounded-md shadow-lg border border-slate-200 py-1 z-50">
            <div className="px-4 py-3 border-b border-slate-100">
              <p className="text-sm font-medium text-slate-900">{user?.email}</p>
              <p className="text-xs text-slate-500 mt-1">
                Role: {user?.roles.join(', ')}
              </p>
            </div>

            <button
              onClick={() => {
                setIsDropdownOpen(false);
                logout();
              }}
              className="w-full flex items-center gap-2 px-4 py-2 text-sm text-red-600 hover:bg-red-50 transition-colors"
            >
              <LogOut className="w-4 h-4" />
              Wyloguj się
            </button>
          </div>
        )}
      </div>
    </header>
  );
}
