import { QueryClient } from '@tanstack/react-query'
import { ErrorBoundary } from 'react-error-boundary'
import { useParams, LoaderFunctionArgs } from 'react-router-dom'

import { ContentLayout } from '@/components/layouts'
import { Spinner } from '@/components/ui/spinner'

import { useActor, getActorQueryOptions } from '@/features/actors/api/get-actor'
import { ActorView } from '@/features/actors/components/actor-view'

export const actorLoader =
  (queryClient: QueryClient) =>
  async ({ params }: LoaderFunctionArgs) => {
    const uniqueId = params.actorUniqueId as string

    const query = getActorQueryOptions(uniqueId)

    const promises = [
      queryClient.getQueryData(query.queryKey) ?? (await queryClient.fetchQuery(query))
    ] as const

    const [actor] = await Promise.all(promises)

    return {
      actor
    }
  }

export const ActorRoute = () => {
  const params = useParams()
  const uniqueId = params.actorUniqueId as string
  const query = useActor({
    uniqueId
  })

  if (query.isLoading) {
    return (
      <div className="flex h-48 w-full items-center justify-center">
        <Spinner size="lg" />
      </div>
    )
  }

  const actor = query?.data

  if (!actor) return null

  return (
    <>
      <ContentLayout title={actor.data.name}>
        <ActorView uniqueId={uniqueId} />
        <div className="mt-8">
          <ErrorBoundary
            fallback={<div>Failed to load data. Try to refresh the page.</div>}
          ></ErrorBoundary>
        </div>
      </ContentLayout>
    </>
  )
}
