import { useState } from 'react';
import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { DataTable, type Column, Pagination, SearchInput, StatusBadge } from '@/components/common';
import { useLinie } from '@/hooks/useLinie';
import { usePagination } from '@/hooks/usePagination';
import type { LiniaListDto } from '@/types/linia.types';

const columns: Column<LiniaListDto>[] = [
  { key: 'numerLinii', header: 'Numer' },
  { key: 'typLinii', header: 'Typ' },
  { key: 'opis', header: 'Opis' },
  { key: 'liczbaWariantow', header: 'Warianty' },
  {
    key: 'aktywna',
    header: 'Status',
    render: (item) => <StatusBadge status={item.aktywna} />,
  },
];

export function LinieListPage() {
  const [search, setSearch] = useState('');
  const { page, pageSize, setPage, setPageSize } = usePagination();

  const { data, isLoading } = useLinie({
    szukaj: search || undefined,
    Page: page,
    PageSize: pageSize,
  });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">Linie</h1>
          <p className="text-slate-500 mt-1">Zarządzaj liniami komunikacyjnymi</p>
        </div>
        <Button>
          <Plus className="w-4 h-4 mr-2" />
          Dodaj linię
        </Button>
      </div>

      <div className="flex items-center gap-4">
        <SearchInput
          value={search}
          onChange={setSearch}
          placeholder="Szukaj linii..."
          className="max-w-sm"
        />
      </div>

      <DataTable
        columns={columns}
        data={data?.items ?? []}
        isLoading={isLoading}
        keyExtractor={(item) => item.id}
        emptyMessage="Nie znaleziono linii"
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
