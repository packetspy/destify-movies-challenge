import { QueryClient } from '@tanstack/react-query'
import { ErrorBoundary } from 'react-error-boundary'
import { useParams, LoaderFunctionArgs } from 'react-router-dom'

import { ContentLayout } from '@/components/layouts'
import { Spinner } from '@/components/ui/spinner'

import { useDirector, getDirectorQueryOptions } from '@/features/directors/api/get-director'
import { DirectorView } from '@/features/directors/components/director-view'

export const Loader =
  (queryClient: QueryClient) =>
  async ({ params }: LoaderFunctionArgs) => {
    const uniqueId = params.directorUniqueId as string

    const query = getDirectorQueryOptions(uniqueId)

    const promises = [
      queryClient.getQueryData(query.queryKey) ?? (await queryClient.fetchQuery(query))
    ] as const

    const [director] = await Promise.all(promises)

    return {
      director
    }
  }

export const Route = () => {
  const params = useParams()
  const uniqueId = params.directorUniqueId as string
  const query = useDirector({
    uniqueId
  })

  if (query.isLoading) {
    return (
      <div className="flex h-48 w-full items-center justify-center">
        <Spinner size="lg" />
      </div>
    )
  }

  const director = query?.data

  if (!director) return null

  return (
    <>
      <ContentLayout title={director.data.name}>
        <DirectorView uniqueId={uniqueId} />
        <div className="mt-8">
          <ErrorBoundary
            fallback={<div>Failed to load data. Try to refresh the page.</div>}
          ></ErrorBoundary>
        </div>
      </ContentLayout>
    </>
  )
}
