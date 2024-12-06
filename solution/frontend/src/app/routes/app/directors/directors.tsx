import { QueryClient } from '@tanstack/react-query'
import { LoaderFunctionArgs } from 'react-router-dom'

import { ContentLayout } from '@/components/layouts'
import { getDirectorsQueryOptions } from '@/features/directors/api/get-directors'
import { DirectorsList } from '@/features/directors/components/directors-list'
import { CreateDirector } from '@/features/directors/components/create-director'

export const Loader =
  (queryClient: QueryClient) =>
  async ({ request }: LoaderFunctionArgs) => {
    const url = new URL(request.url)
    const page = Number(url.searchParams.get('page') || 1)
    const query = getDirectorsQueryOptions({ page })
    return queryClient.getQueryData(query.queryKey)
  }

export const Route = () => {
  return (
    <ContentLayout title="Directors">
      <div className="flex justify-end">
        <CreateDirector />
      </div>
      <div className="mt-4">
        <DirectorsList />
      </div>
    </ContentLayout>
  )
}
