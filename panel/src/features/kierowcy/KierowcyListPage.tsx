import { useState } from 'react';
import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { DataTable, type Column, Pagination, SearchInput, StatusBadge } from '@/components/common';
import { useKierowcy } from '@/hooks/useKierowcy';
import { usePagination } from '@/hooks/usePagination';
import { formatDate } from '@/lib/utils';
import type { KierowcaListDto } from '@/types/kierowca.types';

const columns: Column<KierowcaListDto>[] = [
  { key: 'nrPracownika', header: 'Nr pracownika' },
  { key: 'imie', header: 'Imię' },
  { key: 'nazwisko', header: 'Nazwisko' },
  {
    key: 'dataZatrudnienia',
    header: 'Data zatrudnienia',
    render: (item) => formatDate(item.dataZatrudnienia),
  },
  {
    key: 'aktywny',
    header: 'Status',
    render: (item) => <StatusBadge status={item.aktywny} />,
  },
];

export function KierowcyListPage() {
  const [search, setSearch] = useState('');
  const { page, pageSize, setPage, setPageSize } = usePagination();

  const { data, isLoading } = useKierowcy({
    szukaj: search || undefined,
    Page: page,
    PageSize: pageSize,
  });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">Kierowcy</h1>
          <p className="text-slate-500 mt-1">Zarządzaj kierowcami</p>
        </div>
        <Button>
          <Plus className="w-4 h-4 mr-2" />
          Dodaj kierowcę
        </Button>
      </div>

      <div className="flex items-center gap-4">
        <SearchInput
          value={search}
          onChange={setSearch}
          placeholder="Szukaj kierowcy..."
          className="max-w-sm"
        />
      </div>

      <DataTable
        columns={columns}
        data={data?.items ?? []}
        isLoading={isLoading}
        keyExtractor={(item) => item.id}
        emptyMessage="Nie znaleziono kierowców"
      />

      {data && (
        <Pagination
          currentPage={data.pageNumber}
          totalPages={data.totalPages}
          totalCount={data.totalCount}
          pageSize={pageSize}
          onPageChange={setPage}
          onPageSizeChange={setPageSize}
        />
      )}
    </div>
  );
}
