import {
  LayoutDashboard,
  MapPin,
  Route,
  Users,
  Bus,
  Calendar,
  ClipboardCheck,
  UserCheck,
  Ticket,
  type LucideIcon,
} from 'lucide-react';

export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api/v1';

export const ROLES = {
  Admin: 'Admin',
  Dyspozytor: 'Dyspozytor',
  Kontroler: 'Kontroler',
  Kierowca: 'Kierowca',
  Pasazer: 'Pasazer',
} as const;

export type Role = (typeof ROLES)[keyof typeof ROLES];

export interface NavItem {
  title: string;
  href?: string;
  icon?: LucideIcon;
  roles?: Role[];
  children?: NavItem[];
}

export const NAVIGATION_ITEMS: NavItem[] = [
  {
    title: 'Dashboard',
    href: '/',
    icon: LayoutDashboard,
    roles: ['Admin', 'Dyspozytor', 'Kontroler'],
  },
  {
    title: 'Sieć komunikacyjna',
    roles: ['Admin', 'Dyspozytor'],
    children: [
      { title: 'Przystanki', href: '/przystanki', icon: MapPin },
      { title: 'Linie', href: '/linie', icon: Route },
    ],
  },
  {
    title: 'Flota',
    roles: ['Admin', 'Dyspozytor'],
    children: [
      { title: 'Pojazdy', href: '/pojazdy', icon: Bus },
      { title: 'Kierowcy', href: '/kierowcy', icon: Users },
    ],
  },
  {
    title: 'Operacje',
    roles: ['Admin', 'Dyspozytor'],
    children: [
      { title: 'Realizacje kursów', href: '/realizacje', icon: ClipboardCheck },
      { title: 'Grafiki pracy', href: '/grafiki', icon: Calendar },
    ],
  },
  {
    title: 'Kontrole',
    href: '/kontrole',
    icon: UserCheck,
    roles: ['Admin', 'Kontroler'],
  },
  {
    title: 'Pasażerowie',
    href: '/pasazerowie',
    icon: Ticket,
    roles: ['Admin', 'Dyspozytor'],
  },
];

export const PAGE_SIZE_OPTIONS = [10, 20, 50, 100];
export const DEFAULT_PAGE_SIZE = 20;
