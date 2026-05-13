import { Badge } from '@/components/ui/badge';

type Status = 'active' | 'inactive' | 'pending' | 'completed' | 'error';

interface StatusBadgeProps {
  status: Status | boolean;
  activeLabel?: string;
  inactiveLabel?: string;
}

const STATUS_CONFIG: Record<Status, { label: string; variant: 'success' | 'secondary' | 'warning' | 'default' | 'destructive' }> = {
  active: { label: 'Aktywny', variant: 'success' },
  inactive: { label: 'Nieaktywny', variant: 'secondary' },
  pending: { label: 'Oczekujący', variant: 'warning' },
  completed: { label: 'Zrealizowany', variant: 'default' },
  error: { label: 'Błąd', variant: 'destructive' },
};

export function StatusBadge({ status, activeLabel, inactiveLabel }: StatusBadgeProps) {
  // Handle boolean status
  if (typeof status === 'boolean') {
    const config = status ? STATUS_CONFIG.active : STATUS_CONFIG.inactive;
    const label = status ? (activeLabel || config.label) : (inactiveLabel || config.label);
    return <Badge variant={config.variant}>{label}</Badge>;
  }

  const config = STATUS_CONFIG[status];
  return <Badge variant={config.variant}>{config.label}</Badge>;
}
