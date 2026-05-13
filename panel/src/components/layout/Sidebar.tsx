import { useState } from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import { ChevronDown, Bus } from 'lucide-react';
import { cn } from '@/lib/utils';
import { NAVIGATION_ITEMS, type NavItem } from '@/lib/constants';
import { useAuthStore } from '@/store/authStore';

function NavItemComponent({ item, depth = 0 }: { item: NavItem; depth?: number }) {
  const location = useLocation();
  const { hasAnyRole } = useAuthStore();
  const [isOpen, setIsOpen] = useState(false);

  // Check if user has required role
  if (item.roles && !hasAnyRole(item.roles)) {
    return null;
  }

  const hasChildren = item.children && item.children.length > 0;
  const isChildActive = item.children?.some((child) => child.href === location.pathname);

  const Icon = item.icon;

  if (hasChildren) {
    return (
      <div className="space-y-1">
        <button
          onClick={() => setIsOpen(!isOpen)}
          className={cn(
            'w-full flex items-center justify-between px-3 py-2 text-sm font-medium rounded-lg transition-colors',
            isChildActive || isOpen
              ? 'text-sky-700 bg-sky-50'
              : 'text-slate-600 hover:text-slate-900 hover:bg-slate-100'
          )}
        >
          <div className="flex items-center gap-3">
            {Icon && <Icon className="w-5 h-5 flex-shrink-0" />}
            <span>{item.title}</span>
          </div>
          <ChevronDown
            className={cn('w-4 h-4 transition-transform', isOpen && 'rotate-180')}
          />
        </button>

        {isOpen && (
          <div className="ml-6 space-y-1 border-l-2 border-slate-200 pl-3">
            {item.children!.map((child) => (
              <NavItemComponent key={child.href} item={child} depth={depth + 1} />
            ))}
          </div>
        )}
      </div>
    );
  }

  return (
    <NavLink
      to={item.href!}
      className={({ isActive }) =>
        cn(
          'flex items-center gap-3 px-3 py-2 text-sm font-medium rounded-lg transition-colors',
          depth > 0 ? 'py-1.5' : 'py-2',
          isActive
            ? 'text-sky-700 bg-sky-50'
            : 'text-slate-600 hover:text-slate-900 hover:bg-slate-100'
        )
      }
    >
      {Icon && <Icon className="w-5 h-5 flex-shrink-0" />}
      <span>{item.title}</span>
    </NavLink>
  );
}

export function Sidebar() {
  return (
    <aside className="fixed left-0 top-0 h-full w-64 bg-white border-r border-slate-200 z-40">
      {/* Logo */}
      <div className="h-16 flex items-center gap-3 px-4 border-b border-slate-200">
        <div className="w-10 h-10 bg-sky-500 rounded-lg flex items-center justify-center flex-shrink-0">
          <Bus className="w-6 h-6 text-white" />
        </div>
        <div className="flex flex-col">
          <span className="font-semibold text-slate-900 text-sm">GdzieToSięJechało</span>
          <span className="text-xs text-slate-500">Panel Zarządzania</span>
        </div>
      </div>

      {/* Navigation */}
      <nav className="p-3 space-y-1 overflow-y-auto h-[calc(100%-4rem)]">
        {NAVIGATION_ITEMS.map((item, index) => (
          <NavItemComponent key={item.href || `group-${index}`} item={item} />
        ))}
      </nav>
    </aside>
  );
}
