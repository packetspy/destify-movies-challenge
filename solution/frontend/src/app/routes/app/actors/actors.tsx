import { QueryClient } from '@tanstack/react-query'
import { LoaderFunctionArgs } from 'react-router-dom'

import { ContentLayout } from '@/components/layouts'
import { getActorsQueryOptions } from '@/features/actors/api/get-actors'
import { ActorsList } from '@/features/actors/components/actors-list'
import { CreateActor } from '@/features/actors/components/create-actor'

export const actorsLoader =
  (queryClient: QueryClient) =>
  async ({ request }: LoaderFunctionArgs) => {
    const url = new URL(request.url)
    const query = url.searchParams.get('page') || undefined
    const page = Number(url.searchParams.get('query') || 1)
    const queryActors = getActorsQueryOptions({ query, page })
    return queryClient.getQueryData(queryActors.queryKey)
  }

export const ActorsRoute = () => {
  return (
    <ContentLayout title="Actors">
      <div className="flex justify-end">
        <CreateActor />
      </div>
      <div className="mt-4">
        <ActorsList />
      </div>
    </ContentLayout>
  )
}
